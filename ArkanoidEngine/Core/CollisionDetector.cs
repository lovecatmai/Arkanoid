using ArkanoidEngine.Entities;
using System;

namespace ArkanoidEngine.Core
{
    /// <summary>
    /// Статические методы для определения столкновений между игровыми объектами.
    /// </summary>
    public static class CollisionDetector
    {
        private const float paddleHalfWidthDivisor = 2f;
        private const float hitRatioMinClamp = -1f;
        private const float hitRatioMaxClamp = 1f;
        private const float maxDeflectionAngleInDegrees = 65f;
        private const float degreesToRadiansConversionDivisor = 180f;

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

            var closestX = Clamp(ball.Position.X, rectLeft, rectRight);
            var closestY = Clamp(ball.Position.Y, rectTop, rectBottom);

            var distanceX = ball.Position.X - closestX;
            var distanceY = ball.Position.Y - closestY;

            var squaredDistance = distanceX * distanceX + distanceY * distanceY;
            if (squaredDistance >= ball.Radius * ball.Radius)
                return false;

            var penetrationX = ball.Radius - Math.Abs(distanceX);
            var penetrationY = ball.Radius - Math.Abs(distanceY);

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

            var isHit = CheckBallVsRect(
                ball,
                paddle.Left, paddle.Top, paddle.Right, paddle.Bottom,
                out bool reflectX, out bool reflectY);

            if (!isHit) return false;

            var currentSpeed = (float)Math.Sqrt(
                ball.Velocity.X * ball.Velocity.X +
                ball.Velocity.Y * ball.Velocity.Y);

            var hitPositionRatio = (ball.Position.X - paddle.CenterX) / (paddle.Width / paddleHalfWidthDivisor);
            hitPositionRatio = Clamp(hitPositionRatio, hitRatioMinClamp, hitRatioMaxClamp);

            var maxDeflectionAngle = maxDeflectionAngleInDegrees * (float)(Math.PI / degreesToRadiansConversionDivisor);
            var deflectionAngle    = hitPositionRatio * maxDeflectionAngle;

            newVelocity = new Vector2(
                (float)Math.Sin(deflectionAngle) * currentSpeed,
                -(float)Math.Cos(deflectionAngle) * currentSpeed);

            return true;
        }

        private static float Clamp(float value, float min, float max)
            => value < min ? min : value > max ? max : value;
    }
}
