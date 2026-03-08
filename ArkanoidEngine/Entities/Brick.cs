namespace ArkanoidEngine.Entities
{
    /// <summary>
    /// Represents a single brick on the field.
    /// Position is the top-left corner of the brick.
    /// </summary>
    public class Brick
    {
        public Vector2 Position { get; }
        public float Width { get; }
        public float Height { get; }
        public int MaxHp { get; }
        public int CurrentHp { get; private set; }
        public bool IsAlive => CurrentHp > 0;

        // Convenience bounds
        public float Left   => Position.X;
        public float Right  => Position.X + Width;
        public float Top    => Position.Y;
        public float Bottom => Position.Y + Height;
        public float CenterX => Position.X + Width / 2f;
        public float CenterY => Position.Y + Height / 2f;

        public Brick(Vector2 position, float width, float height, int hp = 1)
        {
            Position = position;
            Width = width;
            Height = height;
            MaxHp = hp;
            CurrentHp = hp;
        }

        public void TakeDamage(int damage)
        {
            CurrentHp -= damage;
            if (CurrentHp < 0) CurrentHp = 0;
        }
    }
}
