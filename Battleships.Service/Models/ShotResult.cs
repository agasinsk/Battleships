using Battleships.Service.Models.Enums;

namespace Battleships.Service.Models
{
    public class ShotResult
    {
        public ShotResultType ShotResultType { get; private set; }

        public ShipType? ShipType { get; private set; }

        public GameField GameField { get; private set; }

        public ShotResult(GameField gameField, ShotResultType shotResultType, ShipType? shipType = null)
        {
            GameField = gameField;
            ShotResultType = shotResultType;
            ShipType = shipType;
        }
    }
}