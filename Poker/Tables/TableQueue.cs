using Poker.Players;
using System.Collections.Concurrent;

namespace Poker.Tables
{
    public partial class Table
    {
        /// <summary>
        /// the Count of Seats taken
        /// </summary>
        public int TakenSeats = 0;

        /// <summary>
        /// the count of Seats which are still Free
        /// </summary>
        public int FreeSeats => Seats.Length - TakenSeats;

        private ConcurrentQueue<Player> _EnrollmentQueue = new ();
        /// <summary>
        /// -1: cancle queue<br/>
        ///  0: ANY Seat<br/>
        /// >0: SelectedSeat
        /// </summary>
        private ConcurrentDictionary<string, int> EnqueuedPlayers = new ConcurrentDictionary<string, int>();
        private ConcurrentDictionary<string, bool> SeatedPlayers = new ConcurrentDictionary<string, bool>();

        private ConcurrentQueue<Player> _SeatReservations = new ();
        /// <summary>
        /// note, 0 means ANY seat.<br/>
        /// > 0 is the requested seat<br/>
        /// in theory you can set -1 to cancle your reservation but use CancelEnrollment instead
        /// </summary>
        /// <param name="player"></param>
        /// <param name="preferredSeat"></param>
        /// <exception cref="Exception"></exception>
        public void Enqueue(Player player, int preferredSeat = 0)
        {
            if (preferredSeat > Seats.Length)
                throw new Exception($"The table only has {Seats.Length} Seats!");

            if (SeatedPlayers.ContainsKey(player.UniqueIdentifier))
                throw new Exception("You are already playing at this table!");

            if (!EnqueuedPlayers.ContainsKey(player.UniqueIdentifier))
            {
                _EnrollmentQueue.Enqueue(player);
                SeatPlayers();
            }
            
            EnqueuedPlayers[player.UniqueIdentifier] = preferredSeat;
        }


        public void CancelEnrollment(Player player)
        {
            if (EnqueuedPlayers.ContainsKey(player.UniqueIdentifier))
            {
                EnqueuedPlayers[player.UniqueIdentifier] = -1;
            }
        }
        private bool TryTakeSeat(Player player, int seatID)
        {
            if (Seats[seatID].Player == null)
            {
                Seats[seatID].Player = player;
                player.Seat = Seats[seatID];
                Interlocked.Increment(ref TakenSeats);
                SeatedPlayers[player.UniqueIdentifier] = true;
                Interlocked.Increment(ref ActivePlayers);
                return true;
            }
            return false;
        }
        private bool TryTakeAnySeat(Player player)
        {
            bool seatFound = false;
            for (int i = 0; i < Seats.Length; i++)
            {
                if (Seats[i].Player == null)
                {
                    if (!TryTakeSeat(player, i))
                    {
                        throw new Exception("Could not assign seat! there seems to be a logic error. Please contact the package creator!");
                    }
                    seatFound = true;
                    break;
                }
            }
            return seatFound;
        }
        /// <summary>
        /// Leaves the Table, Collecting the total seat bank value into the player Bank. The current Bet cannot be claimed back.
        /// </summary>
        public void LeaveTable(int seatID)
        {
            if (Seats[seatID].Player == null)
                return;

            if (TableGame.BettingStructure.RuleSet == Blinds.TableRuleSet.Cash)
            {
                Seats[seatID].BankChips.Clear();
                SeatedPlayers.Remove(Seats[seatID].Player.UniqueIdentifier, out _);
                if (Seats[seatID].SitOutTime == null)
                    Interlocked.Decrement(ref ActivePlayers);
                Seats[seatID].Player.Seat = null;
                Seats[seatID].Player = null;
                Interlocked.Decrement(ref TakenSeats);
                SeatPlayers();
            }
            else if (TableGame.BettingStructure.RuleSet == Blinds.TableRuleSet.Tournament)
            {
                Seats[seatID].SitOut();
            }
            else
            {
                 throw new NotImplementedException(
                     $"The behaviour on leaving a table under the ruleset {TableGame.BettingStructure.RuleSet} is not defined!");
            }
        }

        private object _SeatingLock = new object();
        private void SeatPlayers()
        {
            if (!Monitor.TryEnter(_SeatingLock))
                return; // Another thread is already executing SeatPlayers

            try
            {
                List<Player> reservationStack = new List<Player>();
                
                // seat reserved players first
                for (int i = 0; i < _SeatReservations.Count; i++)
                {
                    if (FreeSeats <= 0)
                        return;
                    if (_SeatReservations.TryDequeue(out Player player) && EnqueuedPlayers.TryGetValue(player.UniqueIdentifier, out int requestedSeat))
                    {
                        if (requestedSeat > 0 && !TryTakeSeat(player, requestedSeat))
                        {
                            // the requested Seat is not available
                            _SeatReservations.Enqueue(player);
                        }
                        else if (requestedSeat == 0 && !TryTakeAnySeat(player))
                        {
                            // seems like no seat is available
                            _SeatReservations.Enqueue(player);
                            return;
                        }
                        else
                        {
                            // seat was found or reservation was cancelled
                            EnqueuedPlayers.Remove(player.UniqueIdentifier, out _);
                        }
                    }                    
                }
                // go through queue
                while (!_EnrollmentQueue.IsEmpty && FreeSeats >= 0 && reservationStack.Count <= FreeSeats)
                {
                    if (_EnrollmentQueue.TryDequeue(out Player player))
                    {
                        int requestedseat = EnqueuedPlayers[player.UniqueIdentifier];
                        
                        if (requestedseat < 0)
                        {
                            // remove players with cancellation requested
                            EnqueuedPlayers.Remove(player.UniqueIdentifier, out _);
                            continue;
                        }
                        if (requestedseat > 0)
                        {
                            // assign players with a preference first (not skipping players without a preference)
                            if (FreeSeats <= 0 || !TryTakeSeat(player, requestedseat))
                                _SeatReservations.Enqueue(player);
                        }
                        else
                        {
                            // temporary stack add to be able to seat preferenced players first while not skipping players without a preference
                            reservationStack.Add(player);
                        }
                    }
                }

                // assign seats to players without a preference
                if (reservationStack.Count > 0)
                {
                    int reservationStackIndex = 0;
                    for (int i = 0; i < Seats.Length; i++)
                    {
                        if (Seats[i].Player == null)
                        {
                            if (!TryTakeSeat(reservationStack[reservationStackIndex], i))
                                throw new Exception("Could not assign seat! there seems to be a logic error. Please contact the package creator!");
                            reservationStackIndex++;
                            if (reservationStackIndex >= reservationStack.Count)
                                break;
                        }
                    }

                    // sanity post check
                    if (reservationStackIndex != reservationStack.Count)
                        throw new Exception("Some players in queue have been skipped!" +
                            "there seems to be a logic error. Please contact the package creator!");
                }
            }
            finally
            {
                Monitor.Exit(_SeatingLock);
            }
        }
    }
}
