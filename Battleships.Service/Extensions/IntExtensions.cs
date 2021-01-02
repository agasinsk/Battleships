using System;

namespace Battleships.Service.Extensions
{
    public static class IntExtensions
    {
        public static char ToLetter(this int value)
        {
            return Convert.ToChar(value + 65);
        }
    }
}