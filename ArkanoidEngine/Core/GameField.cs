namespace ArkanoidEngine.Core
{
    /// <summary>
    /// Размеры игрового поля в пикселях.
    /// </summary>
    public class GameField
    {
        /// <summary>
        /// Ширина поля в пикселях.
        /// </summary>
        public int Width  { get; }

        /// <summary>
        /// Высота поля в пикселях.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Создаёт игровое поле с заданными размерами.
        /// </summary>
        public GameField(int width, int height)
        {
            Width  = width;
            Height = height;
        }
    }
}
