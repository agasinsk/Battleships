using System;

namespace Battleships.ConsoleUI.Models
{
    public class PrintableBoardElement
    {
        public string Value { get; set; }

        public ConsoleColor Color { get; set; }

        public PrintableBoardElement(string value, ConsoleColor consoleColor = ConsoleColor.Gray)
        {
            Value = value;
            Color = consoleColor;
        }

        public PrintableBoardElement(char value, ConsoleColor consoleColor = ConsoleColor.Gray) : this(value.ToString(), consoleColor)
        {
        }

        public PrintableBoardElement(int value, ConsoleColor consoleColor = ConsoleColor.Gray) : this(value.ToString(), consoleColor)
        {
        }

        public override string ToString() => Value;
    }
}