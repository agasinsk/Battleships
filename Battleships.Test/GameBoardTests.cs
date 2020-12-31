using Battleships.Service.Helpers;
using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Battleships.Test
{
    public class GameBoardTests
    {
        private readonly GameBoard _gameBoard;

        public GameBoardTests()
        {
            var ships = new List<Ship>
            {
                new Battleship(OrientationType.Horizontal, new GameField(2, 2)),
                new Destroyer(OrientationType.Vertical, new GameField(3, 4)),
                new Destroyer(OrientationType.Horizontal, new GameField(6, 6)),
            };
            _gameBoard = new GameBoard(10, ships);
        }

        [Theory]
        [InlineData("A5", ShotResultType.Miss, null)]
        [InlineData("B2", ShotResultType.Hit, ShipType.Battleship)]
        [InlineData("C2", ShotResultType.Hit, ShipType.Battleship)]
        [InlineData("D2", ShotResultType.Hit, ShipType.Battleship)]
        [InlineData("E2", ShotResultType.Hit, ShipType.Battleship)]
        [InlineData("F2", ShotResultType.Hit, ShipType.Battleship)]
        [InlineData("C4", ShotResultType.Hit, ShipType.Destroyer)]
        [InlineData("C5", ShotResultType.Hit, ShipType.Destroyer)]
        [InlineData("C6", ShotResultType.Hit, ShipType.Destroyer)]
        [InlineData("C7", ShotResultType.Hit, ShipType.Destroyer)]
        [InlineData("F6", ShotResultType.Hit, ShipType.Destroyer)]
        [InlineData("G6", ShotResultType.Hit, ShipType.Destroyer)]
        [InlineData("H6", ShotResultType.Hit, ShipType.Destroyer)]
        [InlineData("I6", ShotResultType.Hit, ShipType.Destroyer)]
        public void Should_GetCorrectShotResult_WithSingleShot(string shotKey, ShotResultType shotResultType, ShipType? shipType = null)
        {
            // Arrange
            var expectedGameField = ShotKeyInterpreter.GetGameField(shotKey);

            // Act
            var shotResult = _gameBoard.ShootAt(shotKey);

            // Assert
            shotResult.Should().NotBeNull();
            shotResult.ShotResultType.Should().Be(shotResultType);
            shotResult.ShipType.Should().Be(shipType);

            _gameBoard.UsedFields.Should().HaveCount(1);
            _gameBoard.UsedFields.Single().Should().Be(expectedGameField);
        }

        [Fact]
        public void Should_GetSunkShotResult_WhenBattleshipWasSunk()
        {
            // Arrange
            var previousShotResults = new[]
            {
                _gameBoard.ShootAt("B2"),
                _gameBoard.ShootAt("C2"),
                _gameBoard.ShootAt("D2"),
                _gameBoard.ShootAt("E2"),
            };

            // Act
            var shotResult = _gameBoard.ShootAt("F2");

            // Assert
            using var scope = new AssertionScope();

            previousShotResults.Should().HaveCount(4);
            previousShotResults.All(x => x.ShotResultType == ShotResultType.Hit).Should().BeTrue();
            previousShotResults.All(x => x.ShipType == ShipType.Battleship).Should().BeTrue();

            shotResult.Should().NotBeNull();
            shotResult.ShotResultType.Should().Be(ShotResultType.Sunk);
            shotResult.ShipType.Should().Be(ShipType.Battleship);

            _gameBoard.UsedFields.Should().HaveCount(5);
            _gameBoard.UsedFields.First().Should().Be(new GameField(2, 2));
            _gameBoard.UsedFields.Last().Should().Be(new GameField(6, 2));
        }

        [Fact]
        public void Should_GetSunkShotResult_WhenDestroyerWasSunk()
        {
            // Arrange
            var previousShotResults = new[]
            {
                _gameBoard.ShootAt("C4"),
                _gameBoard.ShootAt("C5"),
                _gameBoard.ShootAt("C6"),
            };

            // Act
            var shotResult = _gameBoard.ShootAt("C7");

            // Assert
            using var scope = new AssertionScope();

            previousShotResults.Should().HaveCount(3);
            previousShotResults.All(x => x.ShotResultType == ShotResultType.Hit).Should().BeTrue();
            previousShotResults.All(x => x.ShipType == ShipType.Destroyer).Should().BeTrue();

            shotResult.Should().NotBeNull();
            shotResult.ShotResultType.Should().Be(ShotResultType.Sunk);
            shotResult.ShipType.Should().Be(ShipType.Destroyer);

            _gameBoard.UsedFields.Should().HaveCount(4);
            _gameBoard.UsedFields.First().Should().Be(new GameField(3, 4));
            _gameBoard.UsedFields.Last().Should().Be(new GameField(3, 7));
        }
    }
}