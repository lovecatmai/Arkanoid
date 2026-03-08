using ArkanoidEngine.Entities;
using System;

namespace ArkanoidEngine.Core
{
    /// <summary>
    /// Pure static collision detection utilities.
    /// Does NOT modify any entity state — returns results only.
    /// </summary>
    public static class CollisionDetector
    {
        // -----------------------------------------------------------------------
        // Ball vs Axis-Aligned Bounding Box (AABB)
        // Returns true if collision detected.
        // reflectX / reflectY tell the caller which velocity component to flip.
        // -----------------------------------------------------------------------
        public static bool CheckBallVsRect(
            Ball ball,
            float rectLeft, float rectTop, float rectRight, float rectBottom,
            out bool reflectX, out bool reflectY)
        {
            reflectX = false;
            reflectY = false;

            // Closest point on the rect to the ball center
            float closestX = Clamp(ball.Position.X, rectLeft, rectRight);
            float closestY = Clamp(ball.Position.Y, rectTop, rectBottom);

            float dx = ball.Position.X - closestX;
            float dy = ball.Position.Y - closestY;

            float distSq = dx * dx + dy * dy;
            if (distSq >= ball.Radius * ball.Radius)
                return false;

            // Determine which axis to reflect on by comparing penetration depths.
            // Smaller penetration depth == that's the axis the ball "entered" from.
            float penX = ball.Radius - Math.Abs(dx);
            float penY = ball.Radius - Math.Abs(dy);

            if (penX < penY)
                reflectX = true;
            else
                reflectY = true;

            return true;
        }

        // -----------------------------------------------------------------------
        // Ball vs Paddle — same as rect but adds angle deflection based on hit pos
        // Returns the new velocity after the bounce.
        // -----------------------------------------------------------------------
        public static bool CheckBallVsPaddle(Ball ball, Paddle paddle, out Entities.Vector2 newVelocity)
        {
            newVelocity = ball.Velocity;

            bool hit = CheckBallVsRect(
                ball,
                paddle.Left, paddle.Top, paddle.Right, paddle.Bottom,
                out bool reflectX, out bool reflectY);

            if (!hit) return false;

            // Compute current speed magnitude
            float speed = (float)Math.Sqrt(
                ball.Velocity.X * ball.Velocity.X +
                ball.Velocity.Y * ball.Velocity.Y);

            // Map hit position to [-1, 1]: -1 = far left edge, +1 = far right edge
            float hitRatio = (ball.Position.X - paddle.CenterX) / (paddle.Width / 2f);
            hitRatio = Clamp(hitRatio, -1f, 1f);

            // Max deflection angle from vertical: 65 degrees
            float maxAngle = 65f * (float)(Math.PI / 180f);
            float angle    = hitRatio * maxAngle;

            // New velocity: always goes upward after hitting the paddle
            newVelocity = new Entities.Vector2(
                (float)Math.Sin(angle) * speed,
                -(float)Math.Cos(angle) * speed);

            return true;
        }

        // -----------------------------------------------------------------------
        private static float Clamp(float value, float min, float max)
            => value < min ? min : value > max ? max : value;
    }
}
