using Battleships.Service.Models.Enums;

namespace Battleships.Service.Models
{
    public class Destroyer : Ship
    {
        public const int DefaultSize = 4;

        public override ShipType ShipType => ShipType.Destroyer;

        public override int Size => DefaultSize;

        public Destroyer(OrientationType orientation, GameField startField) : base(orientation, startField)
        {
        }
    }
}