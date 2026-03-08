namespace ArkanoidEngine.Entities
{
    public struct Vector2
    {
        public float X;
        public float Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator *(Vector2 v, float s) => new Vector2(v.X * s, v.Y * s);
        public static Vector2 operator *(float s, Vector2 v) => new Vector2(v.X * s, v.Y * s);

        public override string ToString() => $"({X:F1}, {Y:F1})";
    }
}
