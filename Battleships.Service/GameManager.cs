using Battleships.Service.Builders;
using Battleships.Service.Interfaces;
using Battleships.Service.Models;
using Battleships.Service.Models.Enums;

namespace Battleships.Service
{
    public class GameManager : IGameManager
    {
        private AIPlayer _aiPlayer;

        public GameBoard PlayerBoard { get; private set; }

        public GameBoard AIBoard { get; private set; }

        public int GridSize { get; private set; }

        public GameManager(int gridSize = 10)
        {
            GridSize = gridSize;
        }

        public void SetupGame()
        {
            var gameBoardBuilder = new GameBoardBuilder(GridSize)
                .WithShips(1, ShipType.Battleship)
                .WithShips(2, ShipType.Destroyer);

            PlayerBoard = gameBoardBuilder.Build();
            AIBoard = gameBoardBuilder.Build();

            _aiPlayer = new AIPlayer(AIBoard);
        }

        public WinnerType GetWinner()
        {
            if (AIBoard.AllShipsAreSunk())
            {
                return WinnerType.Player;
            }

            if (PlayerBoard.AllShipsAreSunk())
            {
                return WinnerType.Computer;
            }

            return WinnerType.None;
        }

        public ShotResult PlayPlayerMove(string userShotKey)
        {
            var shotResult = AIBoard.ShootAt(userShotKey);
            PlayerBoard.AddShotResult(shotResult);

            return shotResult;
        }

        public ShotResult PlayAIMove(GameField gameField = null)
        {
            gameField ??= _aiPlayer.GetGameFieldToShoot();

            var shotResult = PlayerBoard.ShootAt(gameField);
            AIBoard.AddShotResult(shotResult);

            return shotResult;
        }
    }
}