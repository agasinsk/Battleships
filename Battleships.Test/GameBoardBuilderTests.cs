using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Battleships.Test
{
    public class GameBoardBuilderTests
    {
        [Theory]
        [InlineData(10, 1, 2)]
        [InlineData(15, 2, 3)]
        [InlineData(20, 0, 0)]
        public void Should_BuildGameBoard_WithSpecifiedCriteria(int gridSize, int battleshipsCount, int destroyersCount)
        {
            // Arrange

            // Act
            GameBoard gameBoard = GameBoardBuilder
                .WithGridSize(gridSize)
                .WithShips(battleshipsCount, ShipType.Battleship)
                .WithShips(destroyersCount, ShipType.Destroyer)
                .Build();

            // Assert
            gameBoard.Should().NotBeNull();
            gameBoard.GridSize.Should().Be(gridSize);

            gameBoard.Ships.Should().NotBeNull();
            gameBoard.Ships.Should().HaveCount(battleshipsCount + destroyersCount);
            gameBoard.Ships.Where(x => x.ShipType == ShipType.Battleship).Should().HaveCount(battleshipsCount);
            gameBoard.Ships.Where(x => x.ShipType == ShipType.Destroyer).Should().HaveCount(destroyersCount);

            // Check if all ships are set apart
            gameBoard.Ships.All(s => !Enumerable.SequenceEqual(s.Fields, gameBoard.Ships.Except(new[] { s }).SelectMany(x => x.Fields)));
        }
    }
}