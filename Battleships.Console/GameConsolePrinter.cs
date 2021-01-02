using Battleships.ConsoleUI.Models;
using Battleships.Service.Extensions;
using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
using System;
using System.Linq;

namespace Battleships.ConsoleUI
{
    internal class GameConsolePrinter
    {
        private readonly PrintableGameBoardConverter _gameBoardConverter;

        public GameConsolePrinter()
        {
            _gameBoardConverter = new PrintableGameBoardConverter();
        }

        internal void PrintGameBoard(GameBoard gameBoard)
        {
            PrintTargetingBoard(gameBoard);
            PrintLineDivider();
            PrintShipBoard(gameBoard);
        }

        internal void PrintWinner(string winner)
        {
            PrintLineDivider();
            Console.WriteLine($"And the winner is {winner}");
        }

        internal void PrintComputerTurn()
        {
            PrintLineDivider();
            Console.Write("Computers turn: ");
        }

        internal void PrintShotResult(ShotResult shotResult)
        {
            switch (shotResult.ShotResultType)
            {
                case ShotResultType.Miss:
                    WriteLineInColor($"{shotResult.ShotResultType}", ConsoleColor.Green);
                    break;

                case ShotResultType.Hit:
                case ShotResultType.Sunk:
                    WriteLineInColor($"{shotResult.ShotResultType} — {shotResult.ShipType}", ConsoleColor.Red);
                    break;
            }
        }

        internal void PrintPlayerTurn()
        {
            PrintLineDivider();
            Console.Write("Shoot at: ");
        }

        internal void PrintGameField(GameField gameField)
        {
            Console.WriteLine($"{gameField.GetShotKey()}");
        }

        private static void PrintLineDivider()
        {
            Console.WriteLine("———————————");
        }

        private void PrintTargetingBoard(GameBoard gameBoard)
        {
            Console.SetCursorPosition(0, 0);
            var targetingBoard = _gameBoardConverter.GetPrintableTargetingBoard(gameBoard);
            PrintBoard(targetingBoard);
        }

        private void PrintShipBoard(GameBoard gameBoard)
        {
            Console.SetCursorPosition(0, gameBoard.GridSize + 2);
            var shipBoard = _gameBoardConverter.GetPrintableShipBoard(gameBoard);
            PrintBoard(shipBoard);
        }

        private void PrintBoard(PrintableBoardElement[][] board)
        {
            for (int column = 0; column < board.First().Length; column++)
            {
                for (int row = 0; row < board[column].Length; row++)
                {
                    var boardElementValue = $"{board[row][column]?.Value ?? "."} ";
                    var boardElementColor = board[row][column]?.Color ?? ConsoleColor.White;
                    WriteInColor(boardElementValue, boardElementColor);
                }

                Console.WriteLine();
            }
        }

        private void WriteLineInColor(string text, ConsoleColor textColor)
        {
            Console.ForegroundColor = textColor;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private void WriteInColor(string text, ConsoleColor textColor)
        {
            Console.ForegroundColor = textColor;
            Console.Write(text);
            Console.ResetColor();
        }
    }
}