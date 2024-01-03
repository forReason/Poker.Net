
using Poker.PhysicalObjects.Chips;

namespace Poker.Tests.PhysicalObjects.Chips
{
    public class PokerChipConversionTests
    {
        [Theory]
        [InlineData(PokerChip.White, 0.01)]
        [InlineData(PokerChip.Red, 0.05)]
        // Add more test data for each PokerChip value
        public void GetChipMicroValue_ReturnsCorrectMicroValue(PokerChip chip, double expectedMicroValue)
        {
            // Act
            var result = Bank.GetChipMicroValue(chip);

            // Assert
            Assert.Equal(expectedMicroValue, result);
        }

        [Theory]
        [InlineData(0.01, 1)]
        [InlineData(0.05, 5)]
        public void ConvertMicroToMacro_Double_ReturnsCorrectMacroValue(double microValue, ulong expectedMacroValue)
        {
            // Act
            var result = Bank.ConvertMicroToMacro(microValue);

            // Assert
            Assert.Equal(expectedMacroValue, result);
        }

        [Theory]
        [InlineData(0.01, 1)]
        [InlineData(0.05, 5)]
        public void ConvertMicroToMacro_Decimal_ReturnsCorrectMacroValue(decimal microValue, ulong expectedMacroValue)
        {
            // Act
            var result = Bank.ConvertMicroToMacro(microValue);

            // Assert
            Assert.Equal(expectedMacroValue, result);
        }

        [Theory]
        [InlineData(1, 0.01)]
        [InlineData(5, 0.05)]
        public void ConvertMacroToMicro_ReturnsCorrectMicroValue(ulong macroValue, double expectedMicroValue)
        {
            // Act
            var result = Bank.ConvertMacroToMicro(macroValue);

            // Assert
            Assert.Equal(expectedMicroValue, result);
        }

        [Theory]
        [InlineData(1, 0.01)]
        [InlineData(5, 0.05)]
        public void ConvertMacroToPreciseMicro_ReturnsCorrectMicroValue(ulong macroValue, decimal expectedMicroValue)
        {
            // Act
            var result = Bank.ConvertMacroToPreciseMicro(macroValue);

            // Assert
            Assert.Equal(expectedMicroValue, result);
        }
    }

}
