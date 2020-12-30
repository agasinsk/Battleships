using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Service.Models
{
    public class GameBoard
    {
        private readonly int[][] _fields;

        public int GridSize { get; private set; }

        public IEnumerable<Ship> Ships { get; private set; }

        public GameBoard(int gridSize, IEnumerable<Ship> ships)
        {
            GridSize = gridSize;
            Ships = ships ?? throw new ArgumentNullException(nameof(ships));

            _fields = Enumerable.Range(1, GridSize)
                .Select(x => Enumerable.Range(1, GridSize).ToArray())
                .ToArray();
        }

        public ShotResult this[string key]
        {
            get => GetShotResult(key);
        }

        private ShotResult GetShotResult(string key)
        {
            throw new NotImplementedException();
        }
    }
}