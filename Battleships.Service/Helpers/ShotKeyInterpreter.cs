using Battleships.Service.Models;
using System;
using System.Linq;

namespace Battleships.Service.Helpers
{
    public class ShotKeyInterpreter
    {
        public static GameField GetGameField(string input, int gridSize)
        {
            ValidateInputOrThrow(input, gridSize);

            var x = ConvertAsciiLetterToInt32(input[0]);
            var y = int.Parse(input[1..]);

            return new GameField(x, y);
        }

        private static void ValidateInputOrThrow(string input, int gridSize)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input cannot be empty");
            }

            if (input.Length < 2)
            {
                throw new ArgumentException("Input must be at least 2 characters long");
            }

            if (!char.IsLetter(input[0]))
            {
                throw new ArgumentException("First character must be a letter");
            }
            else
            {
                if (ConvertAsciiLetterToInt32(input[0]) > gridSize)
                {
                    throw new ArgumentException("Invalid letter provided");
                }
            }

            if (input[1..].Any(x => !char.IsDigit(x)))
            {
                throw new ArgumentException("Only the first character can be a letter");
            }
        }

        private static int ConvertAsciiLetterToInt32(char letter)
        {
            return Convert.ToInt32(letter) - (char.IsUpper(letter) ? 65 : 97) + 1;
        }
    }
}