using Battleships.Service.Helpers;
using FluentAssertions;
using Xunit;

namespace Battleships.Test
{
    public class ShotInputInterpreterTests
    {
        [Theory]
        [InlineData("A1", 1, 1)]
        [InlineData("a1", 1, 1)]
        [InlineData("A5", 1, 5)]
        [InlineData("a5", 1, 5)]
        [InlineData("B6", 2, 6)]
        [InlineData("b6", 2, 6)]
        [InlineData("E10", 5, 10)]
        [InlineData("e10", 5, 10)]
        [InlineData("J22", 10, 22)]
        [InlineData("j22", 10, 22)]
        public void Should_GetCorrectCoordinates_FromShotInput(string input, int expectedX, int expectedY)
        {
            // Act
            var (x, y) = ShotInputInterpreter.GetFieldCoordinates(input);

            // Assert
            x.Should().Be(expectedX);
            y.Should().Be(expectedY);
        }
    }
}