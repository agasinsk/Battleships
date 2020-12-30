using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
using System;

namespace Battleships.Service.Builders
{
    public class ShipBuilder
    {
        private readonly ShipType _shipType;
        private OrientationType _orientation;
        private GameField _startField;

        public ShipBuilder(ShipType shipType)
        {
            _shipType = shipType;
            _orientation = OrientationType.Horizontal;
            _startField = new GameField(1, 1);
        }

        public Ship Build()
        {
            switch (_shipType)
            {
                case ShipType.Battleship:
                    return new Battleship(_orientation, _startField);

                case ShipType.Destroyer:
                    return new Destroyer(_orientation, _startField);

                default:
                    throw new ArgumentException("Unknown ship type was provided");
            }
        }

        public ShipBuilder OnPosition(int x, int y) => OnPosition(new GameField(x, y));

        public ShipBuilder OnPosition(GameField field)
        {
            if (field.X < 0 || field.Y < 0)
            {
                throw new ArgumentException("Invalid ship position");
            }

            _startField = field;
            return this;
        }

        public ShipBuilder WithOrientation(OrientationType orientation)
        {
            _orientation = orientation;
            return this;
        }
    }
}