using Battleships.Service.Models;
using Battleships.Service.Models.Enums;

namespace Battleships.ConsoleUI
{
    public interface IGamePrinter
    {
        void PrintErrorMessage(string message);

        void PrintGameBoard(GameBoard gameBoard);

        void PrintGameField(GameField gameField);

        void PrintShotResult(ShotResult shotResult);

        void PrintPlayerTurn();

        void PrintAIPlayerTurn();

        void PrintWinner(WinnerType winner);
    }
}