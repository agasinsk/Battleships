namespace Battleships.Service.Models
{
    public class GameField
    {
        public int X { get; internal set; }

        public int Y { get; internal set; }

        public bool WasHit { get; private set; }

        public GameField(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(GameField first, GameField second) => first?.Equals(second) ?? false;

        public static bool operator !=(GameField first, GameField second) => !first?.Equals(second) ?? false;

        public override string ToString()
        {
            return $"{X},{Y}";
        }

        public override bool Equals(object obj)
        {
            return obj is GameField field && field.X == X && field.Y == Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() - Y.GetHashCode();
        }

        internal void MarkAsHit()
        {
            WasHit = true;
        }
    }
}