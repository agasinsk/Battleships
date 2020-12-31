using Battleships.Service.Models.Enums;

namespace Battleships.Service.Models
{
    public class ShotResult
    {
        public ShotResultType ShotResultType { get; set; }

        public ShipType? ShipType { get; set; }

        public ShotResult(ShotResultType shotResultType, ShipType? shipType = null)
        {
            ShotResultType = shotResultType;
            ShipType = shipType;
        }
    }
}