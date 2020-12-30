using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
using System;

namespace Battleships.Service.Builders
{
    public class ShipBuilder
    {
        private OrientationType _orientation = OrientationType.Horizontal;

        private ShipType? _shipType;

        private GameField _startField = new GameField(1, 1);

        public ShipBuilder OfType(ShipType shipType)
        {
            _shipType = shipType;
            return this;
        }

        public Ship Build()
        {
            if (!_shipType.HasValue)
            {
                throw new ArgumentException("Ship type must be specified");
            }

            switch (_shipType.Value)
            {
                case ShipType.Battleship:
                    return new Battleship(_orientation, _startField);

                case ShipType.Destroyer:
                    return new Destroyer(_orientation, _startField);

                default:
                    throw new ArgumentException("Unknown ship type was provided");
            }
        }

        public ShipBuilder OnPosition(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                throw new ArgumentException("Invalid ship position");
            }

            _startField = new GameField(x, y);
            return this;
        }

        public ShipBuilder WithOrientation(OrientationType orientation)
        {
            _orientation = orientation;
            return this;
        }

        internal ShipBuilder OnPosition((int x, int y) p) => OnPosition(p.x, p.y);
    }
}