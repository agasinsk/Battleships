using Battleships.Service.Builders;
using Battleships.Service.Models.Enums;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Battleships.Test.Service
{
    public class GameBoardBuilderTests
    {
        [Theory]
        [InlineData(10, 1, 2)]
        [InlineData(10, 2, 3)]
        [InlineData(10, 3, 2)]
        [InlineData(10, 4, 0)]
        [InlineData(15, 2, 3)]
        [InlineData(20, 0, 0)]
        public void Should_BuildGameBoard_WithSpecifiedCriteria(int gridSize, int battleshipsCount, int destroyersCount)
        {
            // Arrange

            // Act
            var gameBoard = new GameBoardBuilder(gridSize)
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

            // Check if no ships are overlapping
            gameBoard.Ships.Any(ship => ship.Fields
                .Any(shipField => gameBoard.Ships.Except(new[] { ship })
                    .SelectMany(x => x.Fields).Contains(shipField))).Should().BeFalse();
        }
    }
}