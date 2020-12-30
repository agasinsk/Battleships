using Battleships.Service.Models.Enums;
using System.Collections.Generic;

namespace Battleships.Service.Models
{
    public abstract class Ship
    {
        public abstract ShipType ShipType { get; }

        public abstract int Size { get; }

        public OrientationType Orientation { get; set; }

        public GameField StartField { get; set; }

        public IEnumerable<GameField> Fields => new[] { StartField };
    }
}