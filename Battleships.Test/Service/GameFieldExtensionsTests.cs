using Battleships.Service.Extensions;
using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Linq;
using Xunit;

namespace Battleships.Test.Service
{
    public class GameFieldExtensionsTests
    {
        [Theory]
        [InlineData(OrientationType.Horizontal, 10)]
        [InlineData(OrientationType.Horizontal, 5)]
        [InlineData(OrientationType.Vertical, 2)]
        [InlineData(OrientationType.Vertical, 8)]
        public void Should_GetProperFields_WhenExpanding(OrientationType orientation, int size)
        {
            // Arrange
            var startField = new GameField(2, 3);
            var expectedLastField = new GameField(
                orientation == OrientationType.Horizontal ? startField.X + size - 1 : startField.X,
                orientation == OrientationType.Horizontal ? startField.Y : startField.Y + size - 1);

            // Act
            var result = startField.Expand(orientation, size);

            // Assert
            using var scope = new AssertionScope();
            result.Should().NotBeNull();
            result.Should().HaveCount(size);

            result.First().Should().Be(startField);
            result.Last().Should().Be(expectedLastField);

            if (orientation == OrientationType.Horizontal)
            {
                result.All(f => f.Y == startField.Y).Should().BeTrue();
            }

            if (orientation == OrientationType.Vertical)
            {
                result.All(f => f.X == startField.X).Should().BeTrue();
            }
        }

        [Theory]
        [InlineData(2, 1, "B1")]
        [InlineData(1, 1, "A1")]
        [InlineData(26, 1, "Z1")]
        [InlineData(1, 10, "A10")]
        [InlineData(2, 111, "B111")]
        [InlineData(26, 111, "Z111")]
        [InlineData(1, 111, "A111")]
        public void Should_GetCorrectShotKey(int x, int y, string expectedKey)
        {
            // Arrange
            var gameField = new GameField(x, y);

            // Act
            var result = gameField.GetShotKey();

            // Assert
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().Be(expectedKey);
        }
    }
}