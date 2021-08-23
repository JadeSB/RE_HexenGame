namespace BoardSystem
{
    public struct Position
    {
        public int X;
        public int Y;
        public int Z;

        public static Position operator +(Position a, Position b)
        {
            return new Position { X = a.X + b.X, Z = a.Z + b.Z };
        }

        public static Position operator -(Position a, Position b)
        {
            return new Position { X = a.X - b.X, Z = a.Z - b.Z };
        }
    }
}
