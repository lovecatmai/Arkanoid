namespace ArkanoidEngine.Entities
{
    /// <summary>
    /// Represents the player-controlled paddle.
    /// Position.X is the horizontal center of the paddle.
    /// Position.Y is the top edge of the paddle.
    /// </summary>
    public class Paddle
    {
        public Vector2 Position { get; set; }
        public float Width { get; }
        public float Height { get; }

        public float Left    => Position.X - Width / 2f;
        public float Right   => Position.X + Width / 2f;
        public float Top     => Position.Y;
        public float Bottom  => Position.Y + Height;
        public float CenterX => Position.X;

        public Paddle(Vector2 position, float width = 100f, float height = 15f)
        {
            Position = position;
            Width = width;
            Height = height;
        }
    }
}
