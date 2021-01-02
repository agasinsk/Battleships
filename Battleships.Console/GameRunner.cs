using Battleships.Service;
using System;

namespace Battleships.ConsoleUI
{
    internal class GameRunner
    {
        private readonly GameConsolePrinter _gamePrinter;
        private readonly GameManager _gameManager;

        public GameRunner()
        {
            _gameManager = new GameManager();
            _gamePrinter = new GameConsolePrinter();
        }

        public void Run()
        {
            _gameManager.SetupGame();
            _gamePrinter.PrintGameBoard(_gameManager.PlayerBoard);

            while (_gameManager.GameIsOn())
            {
                PlayPlayerMove();
                PlayAIMove();

                _gamePrinter.PrintGameBoard(_gameManager.PlayerBoard);
            }

            _gamePrinter.PrintWinner(_gameManager.GetWinner());
        }

        private void PlayAIMove()
        {
            Console.Write("Computers turn: ");
            var aiShotResult = _gameManager.PlayAIMove();
            _gamePrinter.PrintGameField(aiShotResult.GameField);
            _gamePrinter.PrintShotResult(aiShotResult);
        }

        private void PlayPlayerMove()
        {
            Console.Write("Shoot at: ");
            var shotKey = Console.ReadLine();
            var playerShotResult = _gameManager.PlayPlayerMove(shotKey);
            _gamePrinter.PrintShotResult(playerShotResult);
        }
    }
}