using System.Collections.Generic;

namespace Battleships.Service.Models
{
    public class GameBoard
    {
        public int GridSize { get; private set; }

        public IEnumerable<Ship> Ships { get; private set; }
    }
}