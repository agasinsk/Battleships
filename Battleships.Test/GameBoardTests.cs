using Battleships.Service.Helpers;
using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Battleships.Test
{
    public class GameBoardTests
    {
        private GameBoard _gameBoard;

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
            var expectedGameField = ShotInputInterpreter.GetGameField(shotKey);

            // Act
            var shotResult = _gameBoard[shotKey];

            // Assert
            shotResult.Should().NotBeNull();
            shotResult.ShotResultType.Should().Be(shotResultType);
            shotResult.ShipType.Should().Be(shipType);

            _gameBoard.UsedFields.Should().HaveCount(1);
            _gameBoard.UsedFields.Single().Should().Be(expectedGameField);
        }
    }
}