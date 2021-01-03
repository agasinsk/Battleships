using Battleships.ConsoleUI;
using Battleships.ConsoleUI.Models;
using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Battleships.Test.ConsoleUI
{
    public class PrintableGameBoardConverterTests
    {
        private readonly PrintableGameBoardConverter _printableGameBoardConverter;

        public PrintableGameBoardConverterTests()
        {
            _printableGameBoardConverter = new PrintableGameBoardConverter();
        }

        [Fact]
        public void Should_GetCorrectPrintableShipBoard_WithDestroyer()
        {
            // Arrange
            var ships = new List<Ship>()
            {
                new Destroyer(OrientationType.Horizontal, new GameField(2, 2)),
            };

            var gridSize = 10;
            var gameBoard = new GameBoard(gridSize, ships);

            var expectedNotEmptyRows = new[] { 2 };
            var expectedNotEmptyColumns = new[] { 2, 3, 4, 5 };

            // Act
            var result = _printableGameBoardConverter.GetPrintableShipBoard(gameBoard);

            // Assert
            using var scope = new AssertionScope();
            result.Length.Should().Be(gridSize + 1);

            result[1][2].Should().BeNull();
            result[2][2].Should().NotBeNull();
            result[2][2].Value.Should().Be("D");
            result[3][2].Value.Should().Be("D");
            result[4][2].Value.Should().Be("D");
            result[5][2].Value.Should().Be("D");
            result[6][2].Should().BeNull();

            AssertOtherBoardElementsAreEmpty(expectedNotEmptyRows, expectedNotEmptyColumns, result);
        }

        [Fact]
        public void Should_GetCorrectPrintableShipBoard_WithBattleship()
        {
            // Arrange
            var ships = new List<Ship>()
            {
                new Battleship(OrientationType.Horizontal, new GameField(2, 5)),
            };

            var gridSize = 10;
            var gameBoard = new GameBoard(gridSize, ships);

            var expectedNotEmptyRows = new[] { 5 };
            var expectedNotEmptyColumns = new[] { 2, 3, 4, 5, 6 };

            // Act
            var result = _printableGameBoardConverter.GetPrintableShipBoard(gameBoard);

            // Assert
            using var scope = new AssertionScope();
            result.Length.Should().Be(gridSize + 1);

            result[1][5].Should().BeNull();
            result[2][5].Should().NotBeNull();
            result[2][5].Value.Should().Be("B");
            result[3][5].Value.Should().Be("B");
            result[4][5].Value.Should().Be("B");
            result[5][5].Value.Should().Be("B");
            result[6][5].Value.Should().Be("B");
            result[7][5].Should().BeNull();

            AssertOtherBoardElementsAreEmpty(expectedNotEmptyRows, expectedNotEmptyColumns, result);
        }

        [Fact]
        public void Should_GetCorrectPrintableTargetingBoard()
        {
            // Arrange
            var ships = new List<Ship>();

            var gridSize = 10;
            var gameBoard = new GameBoard(gridSize, ships);
            gameBoard.AddShotResult(new ShotResult(new GameField(1, 4), ShotResultType.Miss));
            gameBoard.AddShotResult(new ShotResult(new GameField(5, 4), ShotResultType.Miss));
            gameBoard.AddShotResult(new ShotResult(new GameField(8, 4), ShotResultType.Hit));

            var expectedNotEmptyRows = new[] { 4 };
            var expectedNotEmptyColumns = new[] { 1, 5, 8 };

            // Act
            var result = _printableGameBoardConverter.GetPrintableTargetingBoard(gameBoard);

            // Assert
            using var scope = new AssertionScope();
            result.Length.Should().Be(gridSize + 1);

            result[1][4].Should().NotBeNull();
            result[1][4].Value.Should().Be("O");
            result[5][4].Should().NotBeNull();
            result[5][4].Value.Should().Be("O");
            result[8][4].Should().NotBeNull();
            result[8][4].Value.Should().Be("X");

            AssertOtherBoardElementsAreEmpty(expectedNotEmptyRows, expectedNotEmptyColumns, result);
        }

        private void AssertOtherBoardElementsAreEmpty(int[] expectedNotEmptyRows, int[] expectedNotEmptyColumns, PrintableBoardElement[][] result)
        {
            for (var column = 1; column < result.First().Length; column++)
            {
                for (var row = 1; row < result[column].Length; row++)
                {
                    if (expectedNotEmptyColumns.Contains(column) && expectedNotEmptyRows.Contains(row))
                    {
                        result[column][row].Should().NotBeNull();
                    }
                    else
                    {
                        result[column][row].Should().BeNull();
                    }
                }
            }
        }
    }
}