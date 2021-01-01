using Battleships.Service.Extensions;
using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Service
{
    public class AIPlayer
    {
        private readonly Random _random;
        private readonly int _gridSize;
        private readonly int _maxRetriesCount;

        public AIPlayer(int gridSize)
        {
            _gridSize = gridSize;
            _maxRetriesCount = gridSize * 10;
            _random = new Random();
        }

        public GameField GetGameFieldToShoot(IEnumerable<GameField> usedFields, ShotResult lastShotResult = null)
        {
            if (lastShotResult is null)
            {
                return GetRandomUnusedGameField(usedFields);
            }

            switch (lastShotResult.ShotResultType)
            {
                case ShotResultType.Hit:
                    return GetMostProbableGameField(usedFields, lastShotResult);

                case ShotResultType.Sunk:
                case ShotResultType.Miss:
                default:
                    return GetRandomUnusedGameField(usedFields);
            }
        }

        private GameField GetMostProbableGameField(IEnumerable<GameField> usedFields, ShotResult lastShotResult)
        {
            // TODO: implement more sophisticated logic maybe?
            var orientation = _random.GetRandomOrientation();

            return new GameField(
                lastShotResult.GameField.X + (orientation == OrientationType.Horizontal ? 1 : 0),
                lastShotResult.GameField.Y + (orientation == OrientationType.Vertical ? 1 : 0));
        }

        private GameField GetRandomUnusedGameField(IEnumerable<GameField> usedFields)
        {
            var gameField = new GameField(1, 1);
            var retriesCount = 0;

            do
            {
                if (retriesCount >= _maxRetriesCount)
                {
                    break;
                }

                gameField.X = _random.Next(1, _gridSize + 1);
                gameField.Y = _random.Next(1, _gridSize + 1);

                retriesCount++;
            }
            while (usedFields.Contains(gameField));

            return gameField;
        }
    }
}