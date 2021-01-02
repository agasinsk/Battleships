using Battleships.ConsoleUI.Interfaces;
using System;

namespace Battleships.ConsoleUI
{
    public class ConsoleUserInputProvider : IUserInputProvider
    {
        public string GetUserInput() => Console.ReadLine();
    }
}