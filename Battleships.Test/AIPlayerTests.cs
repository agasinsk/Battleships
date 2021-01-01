using Battleships.Service;
using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
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
            var previousShotResults = Enumerable.Empty<ShotResult>();

            // Act
            var gameField = aiPlayer.GetGameFieldToShoot(previousShotResults);

            // Assert
            gameField.Should().NotBeNull();
            gameField.X.Should().BeLessOrEqualTo(gridSize);
            gameField.X.Should().BePositive();
            gameField.Y.Should().BePositive();
            gameField.Y.Should().BeLessOrEqualTo(gridSize);
            previousShotResults.Any(x => x.GameField == gameField).Should().BeFalse();
        }

        [Fact]
        public void Should_GetUnusedField_WhenFieldsWereUsed()
        {
            // Arrange
            const int gridSize = 2;
            var aiPlayer = new AIPlayer(gridSize);
            var previousShotResults = Enumerable.Range(1, gridSize)
                .SelectMany(x => Enumerable.Range(1, gridSize)
                    .Select(y => new ShotResult(new GameField(x, y), ShotResultType.Miss)))
                .ToList();
            var lastShotResult = previousShotResults.Last();
            previousShotResults.Remove(lastShotResult);

            // Act
            var gameField = aiPlayer.GetGameFieldToShoot(previousShotResults);

            // Assert
            gameField.Should().NotBeNull();
            gameField.Should().Be(lastShotResult.GameField);
            previousShotResults.Any(x => x.GameField == gameField).Should().BeFalse();
        }
    }
}