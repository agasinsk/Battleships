using Battleships.ConsoleUI.Interfaces;
using Battleships.Service.Interfaces;
using Battleships.Service.Models.Enums;
using System;
using System.Threading;

namespace Battleships.ConsoleUI
{
    public class GameRunner
    {
        private readonly IGamePrinter _gamePrinter;
        private readonly IGameManager _gameManager;
        private readonly IUserInputProvider _userInputProvider;

        public GameRunner(IGameManager gameManager, IGamePrinter gamePrinter, IUserInputProvider userInputProvider)
        {
            _gameManager = gameManager ?? throw new ArgumentNullException(nameof(gameManager));
            _gamePrinter = gamePrinter ?? throw new ArgumentNullException(nameof(gamePrinter));
            _userInputProvider = userInputProvider ?? throw new ArgumentNullException(nameof(userInputProvider));
        }

        public void Run()
        {
            _gameManager.SetupGame();
            _gamePrinter.PrintGameBoard(_gameManager.PlayerBoard);

            var winner = WinnerType.None;

            while (winner == WinnerType.None)
            {
                PlayPlayerMove();
                PlayAIMove();

                _gamePrinter.PrintGameBoard(_gameManager.PlayerBoard);
                winner = _gameManager.GetWinner();
            }

            _gamePrinter.PrintWinner(winner);
        }

        private void PlayAIMove()
        {
            _gamePrinter.PrintAIPlayerTurn();
            var aiShotResult = _gameManager.PlayAIMove();
            _gamePrinter.PrintGameField(aiShotResult.GameField);
            _gamePrinter.PrintShotResult(aiShotResult);
        }

        private void PlayPlayerMove()
        {
            _gamePrinter.PrintPlayerTurn();

            var validPlayerTurn = false;

            while (!validPlayerTurn)
            {
                try
                {
                    var shotKey = _userInputProvider.GetUserInput();
                    var playerShotResult = _gameManager.PlayPlayerMove(shotKey);
                    _gamePrinter.PrintShotResult(playerShotResult);
                    validPlayerTurn = true;
                }
                catch (ArgumentException exception)
                {
                    _gamePrinter.PrintErrorMessage(exception.Message);
                    _gamePrinter.PrintPlayerTurn();
                }
            }
        }
    }
}