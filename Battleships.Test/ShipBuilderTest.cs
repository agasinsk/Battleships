using Battleships.Service.Builders;
using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Battleships.Test
{
    public class ShipBuilderTest
    {
        [Theory]
        [InlineData(ShipType.Battleship, OrientationType.Horizontal, 1, 1)]
        [InlineData(ShipType.Battleship, OrientationType.Vertical, 1, 1)]
        [InlineData(ShipType.Battleship, OrientationType.Vertical, 2, 3)]
        [InlineData(ShipType.Battleship, OrientationType.Horizontal, 2, 3)]
        [InlineData(ShipType.Battleship, OrientationType.Vertical, 5, 2)]
        [InlineData(ShipType.Battleship, OrientationType.Horizontal, 5, 2)]
        [InlineData(ShipType.Destroyer, OrientationType.Horizontal, 2, 3)]
        [InlineData(ShipType.Destroyer, OrientationType.Horizontal, 5, 2)]
        [InlineData(ShipType.Destroyer, OrientationType.Vertical, 2, 3)]
        [InlineData(ShipType.Destroyer, OrientationType.Vertical, 5, 2)]
        public void Should_BuildShip_WithGivenCriteria(ShipType shipType, OrientationType orientation, int x, int y)
        {
            // Arrange
            var expectedShipSize = GetExpectedShipSize(shipType);
            var expectedLastField = GetExpectedLastField(orientation, x, y, expectedShipSize);

            // Act
            Ship ship = new ShipBuilder()
                .OfType(shipType)
                .OnPosition(x, y)
                .WithOrientation(orientation)
                .Build();

            // Assert
            ship.Should().NotBeNull();
            ship.Should().BeOfType(GetExpectedType(shipType));

            ship.ShipType.Should().Be(shipType);
            ship.Size.Should().Be(expectedShipSize);
            ship.StartField.X.Should().Be(x);
            ship.StartField.Y.Should().Be(y);

            ship.Orientation.Should().Be(orientation);
            ship.Fields.Should().HaveCount(expectedShipSize);
            ship.Fields.First().Should().Be(ship.StartField);
            ship.Fields.First().X.Should().Be(x);
            ship.Fields.First().Y.Should().Be(y);
            ship.Fields.Last().Should().Be(expectedLastField);
        }

        [Theory]
        [InlineData(ShipType.Battleship)]
        [InlineData(ShipType.Destroyer)]
        public void Should_BuildShip_WithDefaultCriteria(ShipType shipType)
        {
            // Arrange
            var expectedShipSize = GetExpectedShipSize(shipType);
            var expectedLastField = GetExpectedLastField(OrientationType.Horizontal, 1, 1, expectedShipSize);

            // Act
            var ship = new ShipBuilder().OfType(shipType).Build();

            // Assert
            ship.Should().NotBeNull();
            ship.Should().BeOfType(GetExpectedType(shipType));

            ship.ShipType.Should().Be(shipType);
            ship.Size.Should().Be(expectedShipSize);
            ship.StartField.X.Should().Be(1);
            ship.StartField.Y.Should().Be(1);

            ship.Orientation.Should().Be(OrientationType.Horizontal);
            ship.Fields.Should().HaveCount(expectedShipSize);
            ship.Fields.First().Should().Be(ship.StartField);
            ship.Fields.First().X.Should().Be(1);
            ship.Fields.First().Y.Should().Be(1);
            ship.Fields.Last().Should().Be(expectedLastField);
        }

        [Fact]
        public void Should_ThrowException_WhenNoShipTypeIsProvided()
        {
            // Act
            var exception = Assert.Throws<ArgumentException>(() => new ShipBuilder().Build());

            // Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentException>();
            exception.Message.Should().NotBeNullOrEmpty();
            exception.Message.Should().Be("Ship type must be specified");
        }

        private GameField GetExpectedLastField(OrientationType orientation, int x, int y, int expectedShipSize)
        {
            return new GameField(
                orientation == OrientationType.Horizontal ? x + expectedShipSize - 1 : x,
                orientation == OrientationType.Vertical ? y + expectedShipSize - 1 : y);
        }

        private int GetExpectedShipSize(ShipType shipType)
        {
            return shipType == ShipType.Battleship ? Battleship.DefaultSize : Destroyer.DefaultSize;
        }

        private Type GetExpectedType(ShipType shipType)
        {
            return shipType == ShipType.Battleship ? typeof(Battleship) : typeof(Destroyer);
        }
    }
}