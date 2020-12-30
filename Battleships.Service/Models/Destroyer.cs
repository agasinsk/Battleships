using Battleships.Service.Models.Enums;

namespace Battleships.Service.Models
{
    public class Destroyer : Ship
    {
        public override ShipType ShipType => ShipType.Destroyer;

        public override int Size => 4;
    }
}