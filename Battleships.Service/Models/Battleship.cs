using Battleships.Service.Models.Enums;

namespace Battleships.Service.Models
{
    public class Battleship : Ship
    {
        public const int DefaultSize = 5;

        public override ShipType ShipType => ShipType.Battleship;

        public override int Size => DefaultSize;

        public Battleship(OrientationType orientation, GameField startField) : base(orientation, startField)
        {
        }
    }
}