using Battleships.Service.Builders;
using Battleships.Service.Models;
using Battleships.Service.Models.Enums;
using System;

namespace Battleships.Service
{
    public class GameManager
    {
        private AIPlayer _aiPlayer;

        public GameBoard PlayerBoard { get; private set; }

        public GameBoard AIBoard { get; set; }

        public void SetupGame(int gridSize = 10)
        {
            var gameBoardBuilder = new GameBoardBuilder(gridSize)
                .WithShips(1, ShipType.Battleship)
                .WithShips(2, ShipType.Destroyer);

            PlayerBoard = gameBoardBuilder.Build();
            AIBoard = gameBoardBuilder.Build();

            _aiPlayer = new AIPlayer(gridSize);
        }

        public bool GameIsOn() => !AIBoard.AllShipsAreSunk() && !PlayerBoard.AllShipsAreSunk();

        public string GetWinner()
        {
            return AIBoard.AllShipsAreSunk() ? "Computer" : "Player";
        }

        public ShotResult PlayPlayerMove(string userShotKey)
        {
            var shotResult = AIBoard.ShootAt(userShotKey);
            PlayerBoard.ShotResults.Add(shotResult);

            return shotResult;
        }

        public ShotResult PlayAIMove()
        {
            var gameField = _aiPlayer.GetGameFieldToShoot(AIBoard.ShotResults);
            var shotResult = PlayerBoard.ShootAt(gameField);
            AIBoard.ShotResults.Add(shotResult);

            return shotResult;
        }
    }
}