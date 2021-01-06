using Battleships.ConsoleUI.Interfaces;
using System;

namespace Battleships.ConsoleUI.Helpers
{
    public class ConsoleUserInputProvider : IUserInputProvider
    {
        public string GetUserInput() => Console.ReadLine();
    }
}