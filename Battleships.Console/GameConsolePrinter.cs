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
        private readonly int _gridSize;

        private int DefaultAIPlayerTurnPosition => DefaultPlayerTurnPosition + 3;

        private int DefaultPlayerTurnPosition => _gridSize * 2 + 3;

        private int DefaultPlayerTargetingBoardPosition => 0;

        private int DefaultErrorMessagePosition => DefaultPlayerTurnPosition + 2;

        private int DefaultPlayerShipBoardPosition => _gridSize + 2;

        private int DefaultWinnerAnnoucementPosition => DefaultAIPlayerTurnPosition + 2;

        public GameConsolePrinter(int gridSize)
        {
            _gameBoardConverter = new PrintableGameBoardConverter();
            _gridSize = gridSize;
        }

        internal void PrintGameBoard(GameBoard gameBoard)
        {
            PrintTargetingBoard(gameBoard);
            PrintLineDivider();
            PrintShipBoard(gameBoard);
        }

        internal void PrintWinner(WinnerType winner)
        {
            Console.SetCursorPosition(0, DefaultWinnerAnnoucementPosition);
            PrintLineDivider();
           
            WriteLineInColor(winner == WinnerType.Player ? "You won!" : "Unfortunately you lost...",
                winner == WinnerType.Player ? ConsoleColor.Cyan : ConsoleColor.DarkRed);

            Console.WriteLine($"\nPress ENTER to quit...");
            Console.ReadLine();
        }

        internal void PrintAIPlayerTurn()
        {
            Console.SetCursorPosition(0, DefaultAIPlayerTurnPosition);
            PrintLineDivider();
            Console.Write("Computers turn: ");
        }

        internal void PrintShotResult(ShotResult shotResult)
        {
            switch (shotResult.ShotResultType)
            {
                case ShotResultType.Miss:
                    WriteInColor($"{shotResult.ShotResultType}", ConsoleColor.Green);
                    break;

                case ShotResultType.Hit:
                case ShotResultType.Sunk:
                    WriteInColor($"{shotResult.ShotResultType} — {shotResult.ShipType}", ConsoleColor.Red);
                    break;
            }

            ClearToEndOfCurrentLine();
        }

        internal void PrintPlayerTurn()
        {
            Console.SetCursorPosition(0, DefaultPlayerTurnPosition);
            PrintLineDivider();
            Console.Write("\rShoot at: ");
            ClearToEndOfCurrentLine();
        }

        internal void PrintGameField(GameField gameField)
        {
            Console.Write($"{gameField.GetShotKey()}");
            ClearToEndOfCurrentLine();
            Console.WriteLine();
        }

        internal void PrintError(string text)
        {
            Console.SetCursorPosition(0, DefaultErrorMessagePosition);
            WriteInColor(text, ConsoleColor.Red);
            ClearToEndOfCurrentLine();
            Console.WriteLine();
        }

        private static void ClearToEndOfCurrentLine()
        {
            int currentLeft = Console.CursorLeft;
            int currentTop = Console.CursorTop;
            Console.Write(new string(' ', Console.WindowWidth - currentLeft));
            Console.SetCursorPosition(currentLeft, currentTop);
        }

        private void PrintLineDivider()
        {
            Console.Write("——————————————————————");
            ClearToEndOfCurrentLine();
            Console.WriteLine();
        }

        private void PrintTargetingBoard(GameBoard gameBoard)
        {
            Console.SetCursorPosition(0, DefaultPlayerTargetingBoardPosition);

            var targetingBoard = _gameBoardConverter.GetPrintableTargetingBoard(gameBoard);
            PrintBoardElements(targetingBoard);
        }

        private void PrintShipBoard(GameBoard gameBoard)
        {
            Console.SetCursorPosition(0, DefaultPlayerShipBoardPosition);

            var shipBoard = _gameBoardConverter.GetPrintableShipBoard(gameBoard);
            PrintBoardElements(shipBoard);
        }

        private void PrintBoardElements(PrintableBoardElement[][] board)
        {
            for (var column = 0; column < board.First().Length; column++)
            {
                for (var row = 0; row < board[column].Length; row++)
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