using Battleships.ConsoleUI.Models;
using Battleships.Service.Extensions;
using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
using System;
using System.Linq;

namespace Battleships.ConsoleUI.Helpers
{
    public class ConsoleGamePrinter : IGamePrinter
    {
        private readonly PrintableGameBoardConverter _gameBoardConverter;

        private int GridSize { get; set; } = 10;

        private int DefaultAIPlayerTurnPosition => DefaultPlayerTurnPosition + 3;

        private int DefaultPlayerTurnPosition => DefaultPlayerShipBoardPosition + GridSize + 2;

        private int DefaultPlayerTargetingBoardPosition => 6;

        private int DefaultErrorMessagePosition => DefaultPlayerTurnPosition + 2;

        private int DefaultPlayerShipBoardPosition => DefaultPlayerTargetingBoardPosition + GridSize + 2;

        private int DefaultWinnerAnnoucementPosition => DefaultAIPlayerTurnPosition + 3;

        public ConsoleGamePrinter()
        {
            _gameBoardConverter = new PrintableGameBoardConverter();
        }

        public void PrintGameRules()
        {
            Console.Write("Welcome to Battleships! Each player has 1 Battleship ");
            WriteInColor("(B) ", ConsoleColor.DarkCyan);
            Console.Write("and 2 Destroyers ");
            WriteInColor("(D)", ConsoleColor.DarkYellow);
            Console.WriteLine(".");

            Console.WriteLine("Enter coordinates (e.g. \"A2\") to specify a square to target.");
            Console.WriteLine("Shots result in hits, misses or sinks. The game ends when all ships are sunk.");
            Console.WriteLine();
            WriteLineInColor("Good luck!", ConsoleColor.Cyan);
            PrintLineDivider();
        }

        public void PrintGameBoard(GameBoard gameBoard)
        {
            GridSize = gameBoard.GridSize;
            PrintTargetingBoard(gameBoard);
            PrintLineDivider();
            PrintShipBoard(gameBoard);
        }

        public void PrintWinner(WinnerType winner)
        {
            Console.SetCursorPosition(0, DefaultWinnerAnnoucementPosition);
            PrintLineDivider();

            WriteLineInColor(winner == WinnerType.Player ? "You won!" : "Unfortunately you lost...",
                winner == WinnerType.Player ? ConsoleColor.Cyan : ConsoleColor.DarkRed);

            Console.WriteLine($"\nPress ENTER to quit...");
            Console.ReadLine();
        }

        public void PrintAIPlayerTurn()
        {
            Console.SetCursorPosition(0, DefaultAIPlayerTurnPosition);
            PrintLineDivider();
            Console.Write("Computers turn: ");
        }

        public void PrintShotResult(ShotResult shotResult)
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

        public void PrintPlayerTurn()
        {
            Console.SetCursorPosition(0, DefaultPlayerTurnPosition);
            PrintLineDivider();
            Console.Write("\rShoot at: ");
            ClearToEndOfCurrentLine();
        }

        public void PrintGameField(GameField gameField)
        {
            Console.Write($"{gameField.GetShotKey()}");
            ClearToEndOfCurrentLine();
            Console.WriteLine();
        }

        public void PrintErrorMessage(string text)
        {
            Console.SetCursorPosition(0, DefaultErrorMessagePosition);
            WriteInColor(text, ConsoleColor.Red);
            ClearToEndOfCurrentLine();
            Console.WriteLine();
        }

        private void ClearToEndOfCurrentLine()
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