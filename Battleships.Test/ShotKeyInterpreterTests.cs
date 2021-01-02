using Battleships.Service.Helpers;
using FluentAssertions;
using System;
using Xunit;

namespace Battleships.Test
{
    public class ShotKeyInterpreterTests
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
        public void Should_GetCorrectGameField_FromShotKey(string shotKey, int expectedX, int expectedY)
        {
            // Act
            var field = ShotKeyInterpreter.GetGameField(shotKey, gridSize: 10);

            // Assert
            field.Should().NotBeNull();
            field.X.Should().Be(expectedX);
            field.Y.Should().Be(expectedY);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("AAS")]
        [InlineData("a")]
        [InlineData("ab8")]
        [InlineData("8ab1")]
        [InlineData("8")]
        [InlineData("89")]
        [InlineData("8119")]
        [InlineData("$8")]
        [InlineData("S4")]
        [InlineData("K1")]
        public void Should_ThrowValidationException_WhenInputIsInvalid(string input)
        {
            // Act
            var exception = Assert.Throws<ArgumentException>(() => ShotKeyInterpreter.GetGameField(input, gridSize: 10));

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentException>();
        }
    }
}