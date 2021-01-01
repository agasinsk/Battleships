using Battleships.Service.Models.Enums;
using System;

namespace Battleships.Service.Extensions
{
    public static class RandomExtensions
    {
        public static OrientationType GetRandomOrientation(this Random random)
        {
            return random.NextDouble() > 0.5 ? OrientationType.Vertical : OrientationType.Horizontal;
        }
    }
}