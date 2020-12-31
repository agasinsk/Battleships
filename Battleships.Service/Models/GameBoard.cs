using Battleships.Service.Helpers;
using Battleships.Service.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Service.Models
{
    public class GameBoard
    {
        public int GridSize { get; private set; }

        public IEnumerable<Ship> Ships { get; private set; }

        public IList<GameField> UsedFields { get; private set; }

        public GameBoard(int gridSize, IEnumerable<Ship> ships)
        {
            GridSize = gridSize;
            Ships = ships ?? throw new ArgumentNullException(nameof(ships));
            UsedFields = new List<GameField>();
        }

        public ShotResult ShootAt(string shotKey)
        {
            var gameField = ShotKeyInterpreter.GetGameField(shotKey);
            UsedFields.Add(gameField);

            foreach (var ship in Ships)
            {
                var shipField = ship.Fields.FirstOrDefault(f => f == gameField);

                if (shipField != null)
                {
                    shipField.MarkAsHit();

                    return new ShotResult(ship.IsSunk() ? ShotResultType.Sunk : ShotResultType.Hit, ship.ShipType);
                }
            }

            return new ShotResult(ShotResultType.Miss);
        }
    }
}