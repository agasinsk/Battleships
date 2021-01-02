using Battleships.Service;
using Battleships.Service.Extensions;
using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Linq;
using Xunit;

namespace Battleships.Test
{
    public class GameManagerTests
    {
        private readonly GameManager _gameManager;

        public GameManagerTests()
        {
            _gameManager = new GameManager();
        }

        [Fact]
        public void Should_SetupGame_WithDefaultCriteria()
        {
            // Arrange

            // Act
            _gameManager.SetupGame();

            // Assert
            using var scope = new AssertionScope();
            _gameManager.Should().NotBeNull();
            _gameManager.GridSize.Should().Be(10);

            AssertGameBoard(_gameManager.AIBoard);
            AssertGameBoard(_gameManager.PlayerBoard);
        }

        [Theory]
        [InlineData("A2", 1, 2)]
        [InlineData("B5", 2, 5)]
        [InlineData("J10", 10, 10)]
        public void Should_PlayPlayersMove(string userShotKey, int expectedX, int expectedY)
        {
            // Arrange
            _gameManager.SetupGame();
            var expectedGameField = new GameField(expectedX, expectedY);

            // Act
            var shotResult = _gameManager.PlayPlayerMove(userShotKey);

            // Assert
            shotResult.Should().NotBeNull();
            shotResult.GameField.Should().Be(expectedGameField);

            _gameManager.AIBoard.ShotResults.Should().HaveCount(0);
            _gameManager.PlayerBoard.ShotResults.Should().HaveCount(1);
            _gameManager.PlayerBoard.ShotResults.Single().GameField.Should().Be(expectedGameField);
        }

        [Fact]
        public void Should_PlayAIMove()
        {
            // Arrange
            _gameManager.SetupGame();

            // Act
            var shotResult = _gameManager.PlayAIMove();

            // Assert
            shotResult.Should().NotBeNull();

            _gameManager.AIBoard.ShotResults.Should().HaveCount(1);
            _gameManager.PlayerBoard.ShotResults.Should().HaveCount(0);
        }

        [Fact]
        public void Should_ReturnTrue_WhenShipsWereNotSunk()
        {
            // Arrange
            _gameManager.SetupGame();

            // Act
            var result = _gameManager.GameIsOn();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Should_ReturnFalse_WhenShipsWereSunk()
        {
            // Arrange
            _gameManager.SetupGame();

            var fields = _gameManager.AIBoard.Ships.SelectMany(s => s.Fields);

            foreach (var field in fields)
            {
                _gameManager.PlayPlayerMove(field.GetShotKey());
            }

            // Act
            var result = _gameManager.GameIsOn();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Should_DeterminePlayerAsWinner_WhenAllAIShipsWereSunk()
        {
            // Arrange
            _gameManager.SetupGame();

            var fields = _gameManager.AIBoard.Ships.SelectMany(s => s.Fields);

            foreach (var field in fields)
            {
                _gameManager.PlayPlayerMove(field.GetShotKey());
            }

            // Act
            var result = _gameManager.GetWinner();

            // Assert
            result.Should().Be(WinnerType.Player);
        }

        [Fact]
        public void Should_DetermineComputerAsWinner_WhenAllPlayerShipsWereSunk()
        {
            // Arrange
            _gameManager.SetupGame();

            var fields = _gameManager.PlayerBoard.Ships.SelectMany(s => s.Fields);

            foreach (var field in fields)
            {
                _gameManager.PlayAIMove(field);
            }

            // Act
            var result = _gameManager.GetWinner();

            // Assert
            result.Should().Be(WinnerType.Computer);
        }

        private static void AssertGameBoard(GameBoard gameBoard)
        {
            gameBoard.Should().NotBeNull();
            gameBoard.GridSize.Should().Be(10);
            gameBoard.Ships.Should().NotBeNull();
            gameBoard.Ships.Should().HaveCount(3);
            gameBoard.Ships.Where(s => s.ShipType == ShipType.Battleship).Should().HaveCount(1);
            gameBoard.Ships.Where(s => s.ShipType == ShipType.Destroyer).Should().HaveCount(2);
            gameBoard.ShotResults.Should().BeEmpty();
        }
    }
}