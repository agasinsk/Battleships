using Battleships.Service.Extensions;
using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Linq;
using Xunit;

namespace Battleships.Test
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
    }
}