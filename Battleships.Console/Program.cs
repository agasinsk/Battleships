using Battleships.ConsoleUI.Helpers;
using Battleships.Service;

namespace Battleships.ConsoleUI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var battleshipsGame = new GameRunner(new GameManager(), new ConsoleGamePrinter(), new ConsoleUserInputProvider());
            battleshipsGame.Run();
        }
    }
}