using Battleships.Service;
using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Battleships.Test.Service
{
    public class AIPlayerTests
    {
        [Theory]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(50)]
        public void Should_GetRandomUnusedField_WhenShootingFirstTime(int gridSize)
        {
            // Arrange
            var gameBoard = new GameBoard(gridSize, Enumerable.Empty<Ship>());
            var aiPlayer = new AIPlayer(gameBoard);
            var previousShotResults = Enumerable.Empty<ShotResult>();

            // Act
            var gameField = aiPlayer.GetGameFieldToShoot();

            // Assert
            gameField.Should().NotBeNull();
            gameField.X.Should().BeLessOrEqualTo(gridSize);
            gameField.X.Should().BePositive();
            gameField.Y.Should().BePositive();
            gameField.Y.Should().BeLessOrEqualTo(gridSize);
            previousShotResults.Any(x => x.GameField == gameField).Should().BeFalse();
        }

        [Theory]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(50)]
        public void Should_GetTheOnlyUnusedField_WhenAllOtherFieldsWereUsed(int gridSize)
        {
            // Arrange
            var gameBoard = new GameBoard(gridSize, Enumerable.Empty<Ship>());
            var aiPlayer = new AIPlayer(gameBoard);
            var previousShotResults = Enumerable.Range(1, gridSize)
                .SelectMany(x => Enumerable.Range(1, gridSize)
                    .Select(y => new ShotResult(new GameField(x, y), ShotResultType.Miss)))
                .ToList();
            var lastShotResult = previousShotResults.Last();
            previousShotResults.Remove(lastShotResult);

            foreach (var shotResult in previousShotResults)
            {
                gameBoard.AddShotResult(shotResult);
            }

            // Act
            var gameField = aiPlayer.GetGameFieldToShoot();

            // Assert
            gameField.Should().NotBeNull();
            gameField.Should().Be(lastShotResult.GameField);
            previousShotResults.Any(x => x.GameField == gameField).Should().BeFalse();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_GetCorrectMostProbableField_AfterHittingShip(int triedPositionsCount)
        {
            // Arrange
            var gridSize = 10;
            var gameBoard = new GameBoard(gridSize, Enumerable.Empty<Ship>());
            var aiPlayer = new AIPlayer(gameBoard);

            const int x = 2;
            const int y = 3;
            var lastShotResult = new ShotResult(new GameField(x, y), ShotResultType.Hit, ShipType.Battleship);
            gameBoard.AddShotResult(lastShotResult);
            var randomGameField = aiPlayer.GetGameFieldToShoot();

            var nextPositions = new[]
            {
                (1, 0),
                (0, 1),
                (-1, 0),
                (0, -1)
            };

            for (int i = 0; i < triedPositionsCount; i++)
            {
                var nextPosition = nextPositions[i];
                var missedGameField = new GameField(x + nextPosition.Item1, y + nextPosition.Item2);
                gameBoard.AddShotResult(new ShotResult(missedGameField, ShotResultType.Miss));
            }

            var expectedGameField = new GameField(
                x + nextPositions[triedPositionsCount].Item1,
                y + nextPositions[triedPositionsCount].Item2);

            // Act
            var gameField = aiPlayer.GetGameFieldToShoot();

            // Arrange
            gameField.Should().NotBeNull();
            gameField.Should().Be(expectedGameField);
        }

        [Fact]
        public void Should_GetRandomUnusedField_AfterSinkingShip()
        {
            // Arrange
            var gridSize = 10;
            const int x = 2;
            const int y = 3;

            var battleship = new Battleship(OrientationType.Horizontal, new GameField(x, y));
            var gameBoard = new GameBoard(gridSize, new[] { battleship });
            var aiPlayer = new AIPlayer(gameBoard);

            foreach (var field in battleship.Fields)
            {
                var shotResultType = field.X == x + battleship.Size - 1 ? ShotResultType.Sunk : ShotResultType.Hit;
                var shotResult = new ShotResult(field, shotResultType, ShipType.Battleship);
                gameBoard.AddShotResult(shotResult);
                _ = aiPlayer.GetGameFieldToShoot();
            }

            // Act
            var gameField = aiPlayer.GetGameFieldToShoot();

            // Arrange
            gameField.Should().NotBeNull();
            gameBoard.ShotResults.Any(x => x.GameField == gameField).Should().BeFalse();
        }
    }
}