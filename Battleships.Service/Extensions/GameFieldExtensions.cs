using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Service.Extensions
{
    public static class GameFieldExtensions
    {
        public static IEnumerable<GameField> Expand(this GameField field, OrientationType orientation, int size)
        {
            var fields = Enumerable.Range(orientation == OrientationType.Horizontal ? field.X : field.Y, size)
                .Select(point => new GameField(
                    orientation == OrientationType.Horizontal ? point : field.X,
                    orientation == OrientationType.Horizontal ? field.Y : point))
                .ToList();

            return fields;
        }
    }
}