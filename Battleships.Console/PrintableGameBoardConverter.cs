using Battleships.ConsoleUI.Models;
using Battleships.Service.Extensions;
using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.ConsoleUI
{
    public class PrintableGameBoardConverter
    {
        private readonly Dictionary<ShipType, (string tag, ConsoleColor color)> _shipPrinters = new Dictionary<ShipType, (string, ConsoleColor)>
        {
            { ShipType.Battleship, ("B", ConsoleColor.DarkCyan) },
            { ShipType.Destroyer, ("D", ConsoleColor.DarkYellow) },
        };

        private readonly Dictionary<ShotResultType, (string tag, ConsoleColor color)> _shotResultPrinters = new Dictionary<ShotResultType, (string, ConsoleColor)>
        {
            { ShotResultType.Miss, ("O", ConsoleColor.Green) },
            { ShotResultType.Hit, ("X", ConsoleColor.Red) },
            { ShotResultType.Sunk, ("X", ConsoleColor.Red) },
        };

        internal PrintableBoardElement[][] GetPrintableShipBoard(GameBoard gameBoard)
        {
            var gameBoardElements = GetBasePrintableBoard(gameBoard);

            foreach (var ship in gameBoard.Ships)
            {
                foreach (var field in ship.Fields)
                {
                    gameBoardElements[field.X][field.Y] = new PrintableBoardElement(
                        _shipPrinters[ship.ShipType].tag,
                        field.WasHit ? ConsoleColor.Red : _shipPrinters[ship.ShipType].color);
                }
            }

            return gameBoardElements;
        }

        internal PrintableBoardElement[][] GetPrintableTargetingBoard(GameBoard gameBoard)
        {
            var targetingBoard = GetBasePrintableBoard(gameBoard);

            foreach (var shotResult in gameBoard.ShotResults)
            {
                targetingBoard[shotResult.GameField.X][shotResult.GameField.Y] = new PrintableBoardElement(
                    _shotResultPrinters[shotResult.ShotResultType].tag,
                    _shotResultPrinters[shotResult.ShotResultType].color);
            }

            return targetingBoard;
        }

        private PrintableBoardElement[][] GetBasePrintableBoard(GameBoard gameBoard)
        {
            var gridSize = gameBoard.GridSize + 1;
            var gameBoardElements = Enumerable.Range(-1, gridSize)
                .Select(column =>
                {
                    if (column < 0)
                    {
                        return Enumerable.Range(0, gridSize).Select(row => row > 0 ? new PrintableBoardElement(row) : null).ToArray();
                    }
                    else
                    {
                        return Enumerable.Range(-1, gridSize).Select(row => row < 0 ? new PrintableBoardElement(column.ToLetter()) : null).ToArray();
                    }
                })
                .ToArray();

            return gameBoardElements;
        }
    }
}