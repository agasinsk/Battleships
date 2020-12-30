using Battleships.Service.Models.Enums;

namespace Battleships.Service.Models
{
    public class Battleship : Ship
    {
        public override ShipType ShipType => ShipType.Battleship;

        public override int Size => 5;
    }
}