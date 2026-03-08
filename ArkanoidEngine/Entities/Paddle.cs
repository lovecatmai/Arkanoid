namespace ArkanoidEngine.Entities
{
    /// <summary>
    /// Платформа, управляемая игроком для отбивания мяча.
    /// </summary>
    public class Paddle
    {
        /// <summary>
        /// Позиция платформы: X — горизонтальный центр, Y — верхний край.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Ширина платформы в пикселях.
        /// </summary>
        public float Width { get; }

        /// <summary>
        /// Высота платформы в пикселях.
        /// </summary>
        public float Height { get; }

        /// <summary>
        /// Левая граница платформы.
        /// </summary>
        public float Left    => Position.X - Width / 2f;

        /// <summary>
        /// Правая граница платформы.
        /// </summary>
        public float Right   => Position.X + Width / 2f;

        /// <summary>
        /// Верхняя граница платформы.
        /// </summary>
        public float Top     => Position.Y;

        /// <summary>
        /// Нижняя граница платформы.
        /// </summary>
        public float Bottom  => Position.Y + Height;

        /// <summary>
        /// Горизонтальный центр платформы.
        /// </summary>
        public float CenterX => Position.X;

        /// <summary>
        /// Создаёт платформу с заданной позицией и размерами.
        /// </summary>
        public Paddle(Vector2 position, float width = 100f, float height = 15f)
        {
            Position = position;
            Width    = width;
            Height   = height;
        }
    }
}
