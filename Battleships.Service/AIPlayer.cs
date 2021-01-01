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

        public GameField GetGameFieldToShoot(IEnumerable<ShotResult> shotResults)
        {
            if (!shotResults.Any())
            {
                return GetRandomUnusedGameField(shotResults);
            }

            switch (shotResults.Last().ShotResultType)
            {
                case ShotResultType.Hit:
                    return GetMostProbableGameField(shotResults);

                case ShotResultType.Sunk:
                case ShotResultType.Miss:
                default:
                    return GetRandomUnusedGameField(shotResults);
            }
        }

        private GameField GetMostProbableGameField(IEnumerable<ShotResult> shotResults)
        {
            // TODO: implement more sophisticated logic maybe?
            var orientation = _random.GetRandomOrientation();
            var lastShotResult = shotResults.Last();

            return new GameField(
                lastShotResult.GameField.X + (orientation == OrientationType.Horizontal ? 1 : 0),
                lastShotResult.GameField.Y + (orientation == OrientationType.Vertical ? 1 : 0));
        }

        private GameField GetRandomUnusedGameField(IEnumerable<ShotResult> shotResults)
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
            while (shotResults.Select(x => x.GameField).Contains(gameField));

            return gameField;
        }
    }
}