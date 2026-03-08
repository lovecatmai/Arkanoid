namespace ArkanoidEngine.Entities
{
    /// <summary>
    /// Represents the ball in the game.
    /// Position is the center of the ball.
    /// </summary>
    public class Ball
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Radius { get; }
        public int Damage { get; }

        public Ball(Vector2 position, Vector2 velocity, float radius = 8f, int damage = 1)
        {
            Position = position;
            Velocity = velocity;
            Radius = radius;
            Damage = damage;
        }
    }
}
