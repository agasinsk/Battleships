using Battleships.Service.Builders;
using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Battleships.Test
{
    public class ShipBuilderTest
    {
        [Theory]
        [InlineData(ShipType.Battleship, OrientationType.Horizontal, 1, 1)]
        [InlineData(ShipType.Battleship, OrientationType.Vertical, 1, 1)]
        [InlineData(ShipType.Destroyer, OrientationType.Horizontal, 2, 3)]
        [InlineData(ShipType.Destroyer, OrientationType.Vertical, 2, 3)]
        public void Should_BuildShip_WithGivenCriteria(ShipType shipType, OrientationType orientation, int x, int y)
        {
            // Arrange
            var expectedShipType = shipType == ShipType.Battleship ? typeof(Battleship) : typeof(Destroyer);
            var expectedShipSize = shipType == ShipType.Battleship ? Battleship.DefaultSize : Destroyer.DefaultSize;
            var expectedLastFieldX = orientation == OrientationType.Horizontal ? x + expectedShipSize : x;
            var expectedLastFieldY = orientation == OrientationType.Vertical ? y + expectedShipSize : y;

            // Act
            Ship ship = ShipBuilder
                .OfType(shipType)
                .OnPosition(x, y)
                .WithOrientation(orientation)
                .Build();

            // Assert
            ship.Should().NotBeNull();
            ship.Should().BeOfType(expectedShipType);

            ship.ShipType.Should().Be(shipType);
            ship.Size.Should().Be(expectedShipSize);
            ship.StartField.X.Should().Be(x);
            ship.StartField.Y.Should().Be(y);

            ship.Orientation.Should().Be(orientation);
            ship.Fields.Should().HaveCount(expectedShipSize);
            ship.Fields.First().X.Should().Be(x);
            ship.Fields.First().Y.Should().Be(y);
            ship.Fields.Last().X.Should().Be(expectedLastFieldX);
            ship.Fields.Last().Y.Should().Be(expectedLastFieldY);
        }
    }
}