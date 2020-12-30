namespace Battleships.Service.Models
{
    public class GameField
    {
        public int X { get; private set; }

        public int Y { get; private set; }

        public GameField(int x, int y)
        {
            X = x;
            Y = y;
        }

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
    }
}