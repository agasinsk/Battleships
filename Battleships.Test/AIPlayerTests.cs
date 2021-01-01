using Battleships.Service;
using Battleships.Service.Models;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Battleships.Test
{
    public class AIPlayerTests
    {
        [Fact]
        public void Should_GetRandomUnusedField_WhenShootingFirstTime()
        {
            // Arrange
            const int gridSize = 10;
            var aiPlayer = new AIPlayer(gridSize);
            var usedFields = Enumerable.Empty<GameField>();

            // Act
            var gameField = aiPlayer.GetGameFieldToShoot(usedFields);

            // Assert
            gameField.Should().NotBeNull();
            gameField.X.Should().BeLessOrEqualTo(gridSize);
            gameField.X.Should().BePositive();
            gameField.Y.Should().BePositive();
            gameField.Y.Should().BeLessOrEqualTo(gridSize);
            usedFields.Should().NotContain(gameField);
        }

        [Fact]
        public void Should_GetUnusedField_WhenFieldsWereUsed()
        {
            // Arrange
            const int gridSize = 2;
            var aiPlayer = new AIPlayer(gridSize);
            var usedFields = Enumerable.Range(1, gridSize)
                .SelectMany(x => Enumerable.Range(1, gridSize).Select(y => new GameField(x, y)))
                .ToList();
            var expectedGameField = usedFields.Last();
            usedFields.Remove(expectedGameField);

            // Act
            var gameField = aiPlayer.GetGameFieldToShoot(usedFields);

            // Assert
            gameField.Should().NotBeNull();
            gameField.Should().Be(expectedGameField);
            usedFields.Should().NotContain(gameField);
        }
    }
}