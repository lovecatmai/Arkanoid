using static ArkanoidEngine.GameConstants;

namespace ArkanoidEngine.Entities
{
    /// <summary>
    /// Мяч — основной игровой объект, движется по полю и разрушает кирпичи.
    /// </summary>
    public class Ball
    {
        /// <summary>
        /// Позиция центра мяча на игровом поле.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Вектор скорости мяча в пикселях в секунду.
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Радиус мяча в пикселях.
        /// </summary>
        public float Radius { get; }

        /// <summary>
        /// Урон, наносимый мячом за один удар по кирпичу.
        /// </summary>
        public int Damage { get; }

        /// <summary>
        /// Создаёт мяч с заданными параметрами.
        /// </summary>
        public Ball(Vector2 position, Vector2 velocity, float radius = ballRadius, int damage = ballDamage)
        {
            Position = position;
            Velocity = velocity;
            Radius = radius;
            Damage = damage;
        }
    }
}
