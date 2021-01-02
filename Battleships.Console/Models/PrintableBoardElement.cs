using Battleships.Service.Models;
using System;

namespace Battleships.ConsoleUI.Models
{
    internal class PrintableBoardElement
    {
        internal string Value { get; set; }

        internal ConsoleColor Color { get; set; }

        internal PrintableBoardElement(string value, ConsoleColor consoleColor = ConsoleColor.Gray)
        {
            Value = value;
            Color = consoleColor;
        }

        internal PrintableBoardElement(char value, ConsoleColor consoleColor = ConsoleColor.Gray) : this(value.ToString(), consoleColor)
        {
        }

        internal PrintableBoardElement(int value, ConsoleColor consoleColor = ConsoleColor.Gray) : this(value.ToString(), consoleColor)
        {
        }

        public override string ToString() => Value;
    }
}