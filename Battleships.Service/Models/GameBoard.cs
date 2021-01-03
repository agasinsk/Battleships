using Battleships.Service.Helpers;
using Battleships.Service.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Service.Models
{
    public class GameBoard
    {
        private readonly IList<ShotResult> _shotResults;

        public int GridSize { get; private set; }

        public IEnumerable<Ship> Ships { get; private set; }

        public IEnumerable<ShotResult> ShotResults => _shotResults.AsEnumerable();

        public GameBoard(int gridSize, IEnumerable<Ship> ships)
        {
            GridSize = gridSize;
            Ships = ships ?? throw new ArgumentNullException(nameof(ships));
            _shotResults = new List<ShotResult>();
        }

        public ShotResult ShootAt(string shotKey)
        {
            var gameField = ShotKeyInterpreter.GetGameField(shotKey, GridSize);

            return ShootAt(gameField);
        }

        public bool AllShipsAreSunk() => Ships.All(s => s.IsSunk());

        public void AddShotResult(ShotResult shotResult) => _shotResults.Add(shotResult);

        internal ShotResult ShootAt(GameField gameField)
        {
            foreach (var ship in Ships)
            {
                var shipField = ship.Fields.FirstOrDefault(f => f == gameField);

                if (shipField != null)
                {
                    shipField.MarkAsHit();

                    return new ShotResult(gameField, ship.IsSunk() ? ShotResultType.Sunk : ShotResultType.Hit, ship.ShipType);
                }
            }

            return new ShotResult(gameField, ShotResultType.Miss);
        }
    }
}