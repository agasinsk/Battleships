using Battleships.Service.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Service.Models
{
    public abstract class Ship
    {
        public abstract ShipType ShipType { get; }

        public abstract int Size { get; }

        public OrientationType Orientation { get; set; }

        public GameField StartField { get; set; }

        public IEnumerable<GameField> Fields => Enumerable.Range(Orientation == OrientationType.Horizontal ? StartField.X : StartField.Y, Size)
                 .Select(p => new GameField(
                     Orientation == OrientationType.Horizontal ? p : StartField.X,
                     Orientation == OrientationType.Horizontal ? StartField.Y : p))
                 .ToList();

        protected Ship(OrientationType orientation, GameField startField)
        {
            Orientation = orientation;
            StartField = startField ?? throw new ArgumentNullException(nameof(startField));
        }
    }
}