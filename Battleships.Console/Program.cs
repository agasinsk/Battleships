namespace Battleships.ConsoleUI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var battleshipsGame = new GameRunner();
            battleshipsGame.Run();
        }
    }
}