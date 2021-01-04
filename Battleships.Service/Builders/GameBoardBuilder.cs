using Battleships.Service.Extensions;
using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Service.Builders
{
    public class GameBoardBuilder
    {
        private readonly Random _random;
        private readonly int _gridSize;
        private readonly int _maxRetriesCount;
        private readonly List<GameField> _occupiedFields;
        private readonly Dictionary<ShipType, int> _shipsConfiguration;

        public GameBoardBuilder(int gridSize = 10)
        {
            _gridSize = gridSize;
            _maxRetriesCount = gridSize * 100;
            _random = new Random();
            _occupiedFields = new List<GameField>();
            _shipsConfiguration = new Dictionary<ShipType, int>();
        }

        public GameBoardBuilder WithShips(int shipsCount, ShipType shipType)
        {
            _shipsConfiguration[shipType] = shipsCount;
            return this;
        }

        public GameBoard Build()
        {
            var ships = BuildShips();
            var gameBoard = new GameBoard(_gridSize, ships);

            return gameBoard;
        }

        private IEnumerable<Ship> BuildShips()
        {
            var ships = new List<Ship>();

            foreach (var (shipType, shipsCount) in _shipsConfiguration)
            {
                var shipBuilder = new ShipBuilder(shipType);

                var builtShips = Enumerable.Range(1, shipsCount)
                    .Select(x => BuildShip(shipBuilder, shipType));

                ships.AddRange(builtShips);
            }

            _occupiedFields.Clear();

            return ships;
        }

        private Ship BuildShip(ShipBuilder shipBuilder, ShipType shipType)
        {
            var orientation = _random.GetRandomOrientation();

            var ship = shipBuilder
                .WithOrientation(orientation)
                .OnPosition(GetRandomFreePosition(shipType, orientation))
                .Build();

            _occupiedFields.AddRange(ship.Fields);

            return ship;
        }

        private GameField GetRandomFreePosition(ShipType shipType, OrientationType orientation)
        {
            int retriesCount = 0;
            var shipSize = GetShipSize(shipType);

            var maxShipXPosition = orientation == OrientationType.Horizontal
                ? _gridSize - shipSize + 1 : _gridSize + 1;
            var maxShipYPosition = orientation == OrientationType.Vertical
               ? _gridSize - shipSize + 1 : _gridSize + 1;
            var gameField = new GameField(1, 1);

            do
            {
                if (retriesCount >= _maxRetriesCount)
                {
                    throw new ArgumentException($"Could not find free space for {shipType}");
                }

                gameField.X = _random.Next(1, maxShipXPosition);
                gameField.Y = _random.Next(1, maxShipYPosition);

                retriesCount++;
            }
            while (AreFieldsOccupied(gameField, shipSize, orientation));

            return gameField;
        }

        private bool AreFieldsOccupied(GameField gameField, int shipSize, OrientationType orientation)
        {
            var shipFields = gameField.Expand(orientation, shipSize);

            return _occupiedFields.Intersect(shipFields).Any();
        }

        private int GetShipSize(ShipType shipType)
        {
            return shipType == ShipType.Battleship ? Battleship.DefaultSize : Destroyer.DefaultSize;
        }
    }
}