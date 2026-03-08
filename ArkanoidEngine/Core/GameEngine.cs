using ArkanoidEngine.Entities;
using System;
using System.Collections.Generic;

namespace ArkanoidEngine.Core
{
    /// <summary>
    /// Главный игровой движок — управляет логикой, объектами и состоянием игры.
    /// </summary>
    public class GameEngine
    {
        /// <summary>
        /// Размеры игрового поля.
        /// </summary>
        public GameField Field { get; }

        /// <summary>
        /// Текущий мяч.
        /// </summary>
        public Ball Ball { get; private set; }

        /// <summary>
        /// Платформа игрока.
        /// </summary>
        public Paddle Paddle { get; private set; }

        /// <summary>
        /// Список кирпичей текущего уровня.
        /// </summary>
        public IReadOnlyList<Brick> Bricks => brickList;

        /// <summary>
        /// Текущее состояние игры.
        /// </summary>
        public GameState State { get; private set; }

        /// <summary>
        /// Текущий счёт игрока.
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// Вызывается при уничтожении кирпича.
        /// </summary>
        public event Action<Brick> OnBrickDestroyed;

        /// <summary>
        /// Вызывается когда мяч падает за нижний край.
        /// </summary>
        public event Action OnBallLost;

        /// <summary>
        /// Вызывается при уничтожении всех кирпичей.
        /// </summary>
        public event Action OnLevelComplete;

        private List<Brick> brickList;

        private const float ballSpeedPixelsPerSecond  = 320f;
        private const float paddleWidthInPixels       = 100f;
        private const float paddleHeightInPixels      = 14f;
        private const float paddleOffsetFromBottom    = 50f;
        private const float ballOffsetAbovePaddle     = 18f;

        /// <summary>
        /// Создаёт движок с заданными размерами поля и инициализирует игру.
        /// </summary>
        public GameEngine(int fieldWidth = 600, int fieldHeight = 800)
        {
            Field = new GameField(fieldWidth, fieldHeight);
            Initialize();
        }

        /// <summary>
        /// Запускает мяч — переводит состояние в Playing.
        /// </summary>
        public void StartGame()
        {
            if (State == GameState.WaitingToStart)
                State = GameState.Playing;
        }

        /// <summary>
        /// Перемещает платформу по горизонтали; до запуска мяч следует за платформой.
        /// </summary>
        public void SetPaddleX(float x)
        {
            float halfPaddleWidth = Paddle.Width / 2f;
            float clampedX        = Math.Max(halfPaddleWidth, Math.Min(Field.Width - halfPaddleWidth, x));
            Paddle.Position       = new Vector2(clampedX, Paddle.Position.Y);

            if (State == GameState.WaitingToStart)
                Ball.Position = new Vector2(clampedX, Ball.Position.Y);
        }

        /// <summary>
        /// Выполняет один шаг симуляции. Вызывать каждый кадр с дельтой времени в секундах.
        /// </summary>
        public void Update(float deltaTime)
        {
            if (State != GameState.Playing) return;

            MoveBall(deltaTime);
            HandleWallCollisions();
            HandlePaddleCollision();
            HandleBrickCollisions();
            CheckEndConditions();
        }

        /// <summary>
        /// Полностью сбрасывает игру в начальное состояние.
        /// </summary>
        public void Reset()
        {
            Initialize();
        }

        private void Initialize()
        {
            float paddlePositionY = Field.Height - paddleOffsetFromBottom;
            Paddle = new Paddle(
                position: new Vector2(Field.Width / 2f, paddlePositionY),
                width:    paddleWidthInPixels,
                height:   paddleHeightInPixels);

            ResetBallAbovePaddle();

            brickList = LevelBuilder.BuildLevel1(Field.Width);
            Score     = 0;
            State     = GameState.WaitingToStart;
        }

        private void ResetBallAbovePaddle()
        {
            float ballPositionY = Paddle.Top - ballOffsetAbovePaddle;
            Ball = new Ball(
                position: new Vector2(Paddle.CenterX, ballPositionY),
                velocity: new Vector2(0f, -ballSpeedPixelsPerSecond),
                radius:   8f,
                damage:   1);
        }

        private void MoveBall(float deltaTime)
        {
            Ball.Position = Ball.Position + Ball.Velocity * deltaTime;
        }

        private void HandleWallCollisions()
        {
            var currentPosition = Ball.Position;
            var currentVelocity = Ball.Velocity;

            if (currentPosition.X - Ball.Radius < 0)
            {
                currentPosition = new Vector2(Ball.Radius, currentPosition.Y);
                currentVelocity = new Vector2(Math.Abs(currentVelocity.X), currentVelocity.Y);
            }

            if (currentPosition.X + Ball.Radius > Field.Width)
            {
                currentPosition = new Vector2(Field.Width - Ball.Radius, currentPosition.Y);
                currentVelocity = new Vector2(-Math.Abs(currentVelocity.X), currentVelocity.Y);
            }

            if (currentPosition.Y - Ball.Radius < 0)
            {
                currentPosition = new Vector2(currentPosition.X, Ball.Radius);
                currentVelocity = new Vector2(currentVelocity.X, Math.Abs(currentVelocity.Y));
            }

            Ball.Position = currentPosition;
            Ball.Velocity = currentVelocity;

            if (Ball.Position.Y - Ball.Radius > Field.Height)
            {
                State = GameState.Lost;
                OnBallLost?.Invoke();
            }
        }

        private void HandlePaddleCollision()
        {
            if (Ball.Velocity.Y <= 0) return;

            if (CollisionDetector.CheckBallVsPaddle(Ball, Paddle, out Vector2 velocityAfterBounce))
            {
                Ball.Velocity = velocityAfterBounce;
                Ball.Position = new Vector2(Ball.Position.X, Paddle.Top - Ball.Radius - 1f);
            }
        }

        private void HandleBrickCollisions()
        {
            foreach (var brick in brickList)
            {
                if (!brick.IsAlive) continue;

                bool isHit = CollisionDetector.CheckBallVsRect(
                    Ball,
                    brick.Left, brick.Top, brick.Right, brick.Bottom,
                    out bool reflectX, out bool reflectY);

                if (!isHit) continue;

                var velocityAfterHit = Ball.Velocity;
                if (reflectX) velocityAfterHit = new Vector2(-velocityAfterHit.X,  velocityAfterHit.Y);
                if (reflectY) velocityAfterHit = new Vector2( velocityAfterHit.X, -velocityAfterHit.Y);
                Ball.Velocity = velocityAfterHit;

                brick.TakeDamage(Ball.Damage);

                if (!brick.IsAlive)
                {
                    Score += 10;
                    OnBrickDestroyed?.Invoke(brick);
                }

                break;
            }
        }

        private void CheckEndConditions()
        {
            if (State == GameState.Lost) return;

            foreach (var brick in brickList)
                if (brick.IsAlive) return;

            State = GameState.Won;
            OnLevelComplete?.Invoke();
        }
    }
}
