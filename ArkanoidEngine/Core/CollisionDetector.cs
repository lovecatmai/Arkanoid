using ArkanoidEngine.Entities;
using System;

namespace ArkanoidEngine.Core
{
    /// <summary>
    /// Статические методы для определения столкновений между игровыми объектами.
    /// </summary>
    public static class CollisionDetector
    {
        /// <summary>
        /// Проверяет столкновение мяча с прямоугольником и возвращает оси отражения.
        /// </summary>
        public static bool CheckBallVsRect(
            Ball ball,
            float rectLeft,
            float rectTop,
            float rectRight,
            float rectBottom,
            out bool reflectX,
            out bool reflectY
            )
        {
            reflectX = false;
            reflectY = false;

            float closestX = Clamp(ball.Position.X, rectLeft, rectRight);
            float closestY = Clamp(ball.Position.Y, rectTop, rectBottom);

            float distanceX = ball.Position.X - closestX;
            float distanceY = ball.Position.Y - closestY;

            float squaredDistance = distanceX * distanceX + distanceY * distanceY;
            if (squaredDistance >= ball.Radius * ball.Radius)
                return false;

            float penetrationX = ball.Radius - Math.Abs(distanceX);
            float penetrationY = ball.Radius - Math.Abs(distanceY);

            if (penetrationX < penetrationY)
                reflectX = true;
            else
                reflectY = true;

            return true;
        }

        /// <summary>
        /// Проверяет столкновение мяча с платформой и вычисляет новый вектор скорости.
        /// </summary>
        public static bool CheckBallVsPaddle(Ball ball, Paddle paddle, out Vector2 newVelocity)
        {
            newVelocity = ball.Velocity;

            bool isHit = CheckBallVsRect(
                ball,
                paddle.Left, paddle.Top, paddle.Right, paddle.Bottom,
                out bool reflectX, out bool reflectY);

            if (!isHit) return false;

            float currentSpeed = (float)Math.Sqrt(
                ball.Velocity.X * ball.Velocity.X +
                ball.Velocity.Y * ball.Velocity.Y);

            float hitPositionRatio = (ball.Position.X - paddle.CenterX) / (paddle.Width / 2f);
            hitPositionRatio = Clamp(hitPositionRatio, -1f, 1f);

            float maxDeflectionAngle = 65f * (float)(Math.PI / 180f);
            float deflectionAngle    = hitPositionRatio * maxDeflectionAngle;

            newVelocity = new Vector2(
                (float)Math.Sin(deflectionAngle) * currentSpeed,
                -(float)Math.Cos(deflectionAngle) * currentSpeed);

            return true;
        }

        private static float Clamp(float value, float min, float max)
            => value < min ? min : value > max ? max : value;
    }
}
