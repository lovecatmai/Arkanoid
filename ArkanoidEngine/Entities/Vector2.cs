namespace ArkanoidEngine.Entities
{
    /// <summary>
    /// Двумерный вектор с координатами X и Y.
    /// </summary>
    public struct Vector2
    {
        /// <summary>
        /// Координата X.
        /// </summary>
        public float X;

        /// <summary>
        /// Координата Y.
        /// </summary>
        public float Y;

        /// <summary>
        /// Создаёт вектор с заданными координатами.
        /// </summary>
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Сложение двух векторов.
        /// </summary>
        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);

        /// <summary>
        /// Умножение вектора на скаляр.
        /// </summary>
        public static Vector2 operator *(Vector2 v, float scalar) => new Vector2(v.X * scalar, v.Y * scalar);
    }
}
