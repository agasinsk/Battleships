using Battleships.ConsoleUI;
using Battleships.ConsoleUI.Interfaces;
using Battleships.Service.Interfaces;
using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace Battleships.Test.Service
{
    public class GameRunnerTests
    {
        private readonly Mock<IGameManager> _gameManagerMock;
        private readonly Mock<IGamePrinter> _gamePrinterMock;
        private readonly Mock<IUserInputProvider> _userInputProviderMock;
        private readonly GameRunner _gameRunner;

        public GameRunnerTests()
        {
            _gameManagerMock = new Mock<IGameManager>();
            _gamePrinterMock = new Mock<IGamePrinter>();
            _userInputProviderMock = new Mock<IUserInputProvider>();
            _gameRunner = new GameRunner(_gameManagerMock.Object, _gamePrinterMock.Object, _userInputProviderMock.Object);
        }

        [Fact]
        public void Should_RunGameCorrectly()
        {
            // Arrange
            var winner = WinnerType.Player;
            _gameManagerMock.Setup(p => p.GetWinner()).Returns(winner);
            _gameManagerMock.Setup(p => p.PlayAIMove(null)).Returns(new ShotResult(new GameField(1, 1), ShotResultType.Miss));

            var shotKey = "B2";
            _userInputProviderMock.Setup(p => p.GetUserInput()).Returns(shotKey);

            // Act
            _gameRunner.Run();

            // Arrange
            _gameManagerMock.Verify(p => p.SetupGame(), Times.Once);
            _gameManagerMock.Verify(p => p.PlayerBoard, Times.Exactly(2));
            _gameManagerMock.Verify(p => p.PlayPlayerMove(shotKey), Times.Once);
            _gameManagerMock.Verify(p => p.PlayAIMove(null), Times.Once);
            _gameManagerMock.Verify(p => p.GetWinner(), Times.Once);

            _gamePrinterMock.Verify(p => p.PrintGameBoard(It.IsAny<GameBoard>()), Times.Exactly(2));
            _gamePrinterMock.Verify(p => p.PrintPlayerTurn(), Times.Once);
            _gamePrinterMock.Verify(p => p.PrintAIPlayerTurn(), Times.Once);
            _gamePrinterMock.Verify(p => p.PrintGameField(It.IsAny<GameField>()), Times.Once);
            _gamePrinterMock.Verify(p => p.PrintShotResult(It.IsAny<ShotResult>()), Times.Exactly(2));
            _gamePrinterMock.Verify(p => p.PrintWinner(winner), Times.Once);

            _gamePrinterMock.Verify(p => p.PrintErrorMessage(It.IsAny<string>()), Times.Never);

            _userInputProviderMock.Verify(p => p.GetUserInput(), Times.Once);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(50)]
        public void Should_RunGameCorrectly_WithMultipleTurns(int totalTurnsCount)
        {
            // Arrange
            var turnCount = 0;
            var winner = WinnerType.Player;
            _gameManagerMock.Setup(p => p.GetWinner())
                .Callback(() =>
                {
                    turnCount++;
                })
                .Returns(() =>
                {
                    return turnCount == totalTurnsCount ? winner : WinnerType.None;
                });

            _gameManagerMock.Setup(p => p.PlayAIMove(null)).Returns(new ShotResult(new GameField(1, 1), ShotResultType.Miss));

            var shotKey = "B2";
            _userInputProviderMock.Setup(p => p.GetUserInput()).Returns(shotKey);

            // Act
            _gameRunner.Run();

            // Arrange
            _gameManagerMock.Verify(p => p.SetupGame(), Times.Once);
            _gameManagerMock.Verify(p => p.PlayerBoard, Times.Exactly(totalTurnsCount + 1));
            _gameManagerMock.Verify(p => p.PlayPlayerMove(shotKey), Times.Exactly(totalTurnsCount));
            _gameManagerMock.Verify(p => p.PlayAIMove(null), Times.Exactly(totalTurnsCount));
            _gameManagerMock.Verify(p => p.GetWinner(), Times.Exactly(totalTurnsCount));

            _gamePrinterMock.Verify(p => p.PrintGameBoard(It.IsAny<GameBoard>()), Times.Exactly(totalTurnsCount + 1));
            _gamePrinterMock.Verify(p => p.PrintPlayerTurn(), Times.Exactly(totalTurnsCount));
            _gamePrinterMock.Verify(p => p.PrintAIPlayerTurn(), Times.Exactly(totalTurnsCount));
            _gamePrinterMock.Verify(p => p.PrintGameField(It.IsAny<GameField>()), Times.Exactly(totalTurnsCount));
            _gamePrinterMock.Verify(p => p.PrintShotResult(It.IsAny<ShotResult>()), Times.Exactly(2 * totalTurnsCount));
            _gamePrinterMock.Verify(p => p.PrintWinner(winner), Times.Once);

            _gamePrinterMock.Verify(p => p.PrintErrorMessage(It.IsAny<string>()), Times.Never);

            _userInputProviderMock.Verify(p => p.GetUserInput(), Times.Exactly(totalTurnsCount));
        }

        [Theory]
        [InlineData("S1", null, "AA", "101", "9A", "A2")]
        [InlineData("Z1", "", "89", "9AAS", "A2")]
        public void Should_StopOnUserInput_IfItIsInvalid(params string[] userInputs)
        {
            // Arrange
            var winner = WinnerType.Player;
            _gameManagerMock.Setup(p => p.GetWinner()).Returns(winner);
            _gameManagerMock.Setup(p => p.PlayAIMove(null)).Returns(new ShotResult(new GameField(1, 1), ShotResultType.Miss));

            var userInputCallCount = 0;
            _userInputProviderMock.Setup(p => p.GetUserInput())
                .Returns(() => userInputs[userInputCallCount])
                .Callback(() => { userInputCallCount++; });

            var errorMessage = "Invalid user input";
            foreach (var userInput in userInputs.Take(userInputs.Length - 1))
            {
                _gameManagerMock.Setup(p => p.PlayPlayerMove(userInput)).Throws(new ArgumentException(errorMessage));
            }

            // Act
            _gameRunner.Run();

            // Arrange
            _gameManagerMock.Verify(p => p.SetupGame(), Times.Once);
            _gameManagerMock.Verify(p => p.PlayerBoard, Times.Exactly(2));
            _gameManagerMock.Verify(p => p.PlayPlayerMove(It.IsAny<string>()), Times.Exactly(userInputs.Length));
            foreach (var userInput in userInputs)
            {
                _gameManagerMock.Verify(p => p.PlayPlayerMove(userInput), Times.Once);
            }

            _gameManagerMock.Verify(p => p.PlayAIMove(null), Times.Once);
            _gameManagerMock.Verify(p => p.GetWinner(), Times.Once);

            _gamePrinterMock.Verify(p => p.PrintGameBoard(It.IsAny<GameBoard>()), Times.Exactly(2));
            _gamePrinterMock.Verify(p => p.PrintPlayerTurn(), Times.Exactly(userInputs.Length));
            _gamePrinterMock.Verify(p => p.PrintAIPlayerTurn(), Times.Once);
            _gamePrinterMock.Verify(p => p.PrintGameField(It.IsAny<GameField>()), Times.Once);
            _gamePrinterMock.Verify(p => p.PrintShotResult(It.IsAny<ShotResult>()), Times.Exactly(2));
            _gamePrinterMock.Verify(p => p.PrintWinner(winner), Times.Once);

            _gamePrinterMock.Verify(p => p.PrintErrorMessage(It.IsAny<string>()), Times.Exactly(userInputs.Length - 1));
            _gamePrinterMock.Verify(p => p.PrintErrorMessage(errorMessage), Times.Exactly(userInputs.Length - 1));

            _userInputProviderMock.Verify(p => p.GetUserInput(), Times.Exactly(userInputs.Length));
        }
    }
}