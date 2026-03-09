using static ArkanoidEngine.GameConstants;

namespace ArkanoidEngine.Entities
{
    /// <summary>
    /// Кирпич на игровом поле — получает урон от мяча и разрушается.
    /// </summary>
    public class Brick
    {
        /// <summary>
        /// Позиция левого верхнего угла кирпича.
        /// </summary>
        public Vector2 Position { get; }

        /// <summary>
        /// Ширина кирпича в пикселях.
        /// </summary>
        public float Width { get; }

        /// <summary>
        /// Высота кирпича в пикселях.
        /// </summary>
        public float Height { get; }

        /// <summary>
        /// Максимальное количество очков здоровья.
        /// </summary>
        public int MaxHp { get; }

        /// <summary>
        /// Текущее количество очков здоровья.
        /// </summary>
        public int CurrentHp { get; private set; }

        /// <summary>
        /// Признак того, что кирпич ещё не разрушен.
        /// </summary>
        public bool IsAlive => CurrentHp > brickMinHP;

        /// <summary>
        /// Левая граница кирпича.
        /// </summary>
        public float Left => Position.X;

        /// <summary>
        /// Правая граница кирпича.
        /// </summary>
        public float Right => Position.X + Width;

        /// <summary>
        /// Верхняя граница кирпича.
        /// </summary>
        public float Top => Position.Y;

        /// <summary>
        /// Нижняя граница кирпича.
        /// </summary>
        public float Bottom => Position.Y + Height;

        /// <summary>
        /// Создаёт кирпич с заданной позицией, размерами и количеством HP.
        /// </summary>
        public Brick(Vector2 position, float width, float height, int hp = brickMaxHP)
        {
            Position = position;
            Width = width;
            Height = height;
            MaxHp = hp;
            CurrentHp = hp;
        }

        /// <summary>
        /// Наносит урон кирпичу, уменьшая его текущее HP.
        /// </summary>
        public void TakeDamage(int damage)
        {
            CurrentHp -= damage;
            if (CurrentHp < brickMinHP)
            {
                CurrentHp = brickMinHP;
            }
        }
    }
}
