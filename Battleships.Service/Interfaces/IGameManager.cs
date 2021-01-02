using Battleships.Service.Models;
using Battleships.Service.Models.Enums;

namespace Battleships.Service.Interfaces
{
    public interface IGameManager
    {
        GameBoard AIBoard { get; }

        int GridSize { get; }

        GameBoard PlayerBoard { get; }

        WinnerType GetWinner();

        ShotResult PlayAIMove(GameField gameField = null);

        ShotResult PlayPlayerMove(string userShotKey);

        void SetupGame();
    }
}