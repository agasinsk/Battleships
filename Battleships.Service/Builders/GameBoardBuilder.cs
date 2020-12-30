﻿using Battleships.Service.Extensions;
using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Service.Builders
{
    public class GameBoardBuilder
    {
        private readonly List<Ship> _ships;
        private readonly Random _random;
        private readonly int _gridSize;
        private ShipBuilder _shipBuilder;
        private List<GameField> _occupiedFields;
        private int _maxRetriesCount => _gridSize * 10;

        public GameBoardBuilder(int gridSize = 10)
        {
            _gridSize = gridSize;
            _random = new Random();
            _ships = new List<Ship>();
            _occupiedFields = new List<GameField>();
        }

        public GameBoardBuilder WithShips(int shipsCount, ShipType shipType)
        {
            _shipBuilder = new ShipBuilder(shipType);

            var ships = Enumerable.Range(1, shipsCount)
                .Select(x =>
                {
                    var orientation = GetRandomOrientation();

                    var ship = _shipBuilder
                        .WithOrientation(orientation)
                        .OnPosition(GetRandomFreePosition(shipType, orientation))
                        .Build();
                    _occupiedFields.AddRange(ship.Fields);

                    return ship;
                });

            _ships.AddRange(ships);

            return this;
        }

        public GameBoard Build()
        {
            var gameBoard = new GameBoard(_gridSize, _ships);

            return gameBoard;
        }

        private GameField GetRandomFreePosition(ShipType shipType, OrientationType orientation)
        {
            int retriesCount = 0;
            var shipSize = GetShipSize(shipType);
            var maxShipStartPosition = _gridSize - shipSize + 1;
            var gameField = new GameField(0, 0);

            do
            {
                if (retriesCount >= _maxRetriesCount)
                {
                    throw new ArgumentException($"Could not find free space for {shipType}");
                }

                gameField.X = _random.Next(1, maxShipStartPosition);
                gameField.Y = _random.Next(1, maxShipStartPosition);

                retriesCount++;
            }
            while (AreFieldsOccupied(gameField, shipSize, orientation));

            return gameField;
        }

        private bool AreFieldsOccupied(GameField gameField, int shipSize, OrientationType orientation)
        {
            var shipFields = gameField.Expand(orientation, shipSize);

            return shipFields.Any(field => IsFieldOccupied(field));
        }

        private bool IsFieldOccupied(GameField field)
        {
            return _occupiedFields.Any(f => f == field);
        }

        private int GetShipSize(ShipType shipType)
        {
            return shipType == ShipType.Battleship ? Battleship.DefaultSize : Destroyer.DefaultSize;
        }

        private OrientationType GetRandomOrientation()
        {
            return _random.NextDouble() > 0.5 ? OrientationType.Vertical : OrientationType.Horizontal;
        }
    }
}