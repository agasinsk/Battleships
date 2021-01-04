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
        private readonly GameBoard _gameBoard;
        private readonly int _maxRetriesCount;

        private readonly IEnumerable<(int x, int y)> nextPossibleFieldPositions = new[]
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0, -1)
        };

        private IEnumerable<GameField> UsedGameFields => _gameBoard?.ShotResults.Select(x => x.GameField);

        private ShotResult LastShotResult => _gameBoard.ShotResults.Last();

        private GameField LastHitGameField { get; set; }

        public AIPlayer(GameBoard gameBoard)
        {
            _random = new Random();
            _gameBoard = gameBoard;
            _maxRetriesCount = gameBoard.GridSize * 100;
        }

        public GameField GetGameFieldToShoot()
        {
            if (!UsedGameFields.Any())
            {
                return GetRandomUnusedGameField();
            }

            switch (LastShotResult.ShotResultType)
            {
                case ShotResultType.Hit:
                    LastHitGameField = LastShotResult.GameField;
                    return GetMostProbableGameField();

                case ShotResultType.Miss:
                    return GetMostProbableGameField();

                case ShotResultType.Sunk:
                    LastHitGameField = null;
                    return GetRandomUnusedGameField();

                default:
                    return GetRandomUnusedGameField();
            }
        }

        private GameField GetMostProbableGameField()
        {
            if (LastHitGameField is null)
            {
                return GetRandomUnusedGameField();
            }

            var nextGameField = new GameField(1, 1);

            foreach (var (nextX, nextY) in nextPossibleFieldPositions)
            {
                nextGameField.X = Math.Max(1, Math.Min(LastHitGameField.X + nextX, _gameBoard.GridSize));
                nextGameField.Y = Math.Max(1, Math.Min(LastHitGameField.Y + nextY, _gameBoard.GridSize));

                if (!UsedGameFields.Contains(nextGameField))
                {
                    break;
                }
            }

            return nextGameField;
        }

        private GameField GetRandomUnusedGameField()
        {
            var gameField = new GameField(1, 1);
            var retriesCount = 0;
            var maxPosition = _gameBoard.GridSize + 1;

            do
            {
                if (retriesCount >= _maxRetriesCount)
                {
                    break;
                }

                gameField.X = _random.Next(1, maxPosition);
                gameField.Y = _random.Next(1, maxPosition);

                retriesCount++;
            }
            while (UsedGameFields.Contains(gameField));

            return gameField;
        }
    }
}