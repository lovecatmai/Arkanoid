using ArkanoidEngine.Entities;
using System;
using System.Collections.Generic;

namespace ArkanoidEngine.Core
{
    /// <summary>
    /// Core game engine. No UI, no rendering, no platform code.
    /// Call Update(deltaTime) each frame.
    /// Read State, Ball, Paddle, Bricks to render.
    /// </summary>
    public class GameEngine
    {
        // -----------------------------------------------------------------------
        // Public read-only state
        // -----------------------------------------------------------------------
        public GameField            Field     { get; }
        public Ball                 Ball      { get; private set; }
        public Paddle               Paddle    { get; private set; }
        public IReadOnlyList<Brick> Bricks    => _bricks;
        public GameState            State     { get; private set; }
        public int                  Score     { get; private set; }

        // -----------------------------------------------------------------------
        // Events — subscribe in your UI layer
        // -----------------------------------------------------------------------
        public event Action<Brick> OnBrickDestroyed;
        public event Action        OnBallLost;
        public event Action        OnLevelComplete;

        // -----------------------------------------------------------------------
        // Private
        // -----------------------------------------------------------------------
        private List<Brick> _bricks;

        private const float BallSpeed       = 320f;   // pixels per second
        private const float PaddleWidth     = 100f;
        private const float PaddleHeight    = 14f;
        private const float PaddleOffsetY   = 50f;    // distance from bottom of field to paddle top
        private const float BallOffsetAbove = 18f;    // distance from paddle top to ball center at start

        // -----------------------------------------------------------------------
        public GameEngine(int fieldWidth = 600, int fieldHeight = 800)
        {
            Field = new GameField(fieldWidth, fieldHeight);
            Initialize();
        }

        // -----------------------------------------------------------------------
        // Initialise / reset everything
        // -----------------------------------------------------------------------
        private void Initialize()
        {
            float paddleY = Field.Height - PaddleOffsetY;
            Paddle = new Paddle(
                position: new Vector2(Field.Width / 2f, paddleY),
                width:    PaddleWidth,
                height:   PaddleHeight);

            ResetBall();

            _bricks = LevelBuilder.BuildLevel1(Field.Width);
            Score   = 0;
            State   = GameState.WaitingToStart;
        }

        private void ResetBall()
        {
            float ballY = Paddle.Top - BallOffsetAbove;
            Ball = new Ball(
                position: new Vector2(Paddle.CenterX, ballY),
                velocity: new Vector2(0f, -BallSpeed),
                radius:   8f,
                damage:   1);
        }

        // -----------------------------------------------------------------------
        // Public API
        // -----------------------------------------------------------------------

        /// <summary>Launch the ball. Call when player clicks / presses Space.</summary>
        public void StartGame()
        {
            if (State == GameState.WaitingToStart)
                State = GameState.Playing;
        }

        /// <summary>
        /// Move the paddle to the given X coordinate (in game-world pixels).
        /// While waiting to start, the ball follows the paddle.
        /// </summary>
        public void SetPaddleX(float x)
        {
            float halfW    = Paddle.Width / 2f;
            float clampedX = Math.Max(halfW, Math.Min(Field.Width - halfW, x));
            Paddle.Position = new Vector2(clampedX, Paddle.Position.Y);

            if (State == GameState.WaitingToStart)
                Ball.Position = new Vector2(clampedX, Ball.Position.Y);
        }

        /// <summary>Advance the simulation by deltaTime seconds.</summary>
        public void Update(float deltaTime)
        {
            if (State != GameState.Playing) return;

            MoveBall(deltaTime);
            HandleWallCollisions();
            HandlePaddleCollision();
            HandleBrickCollisions();
            CheckEndConditions();
        }

        /// <summary>Fully reset game to initial state.</summary>
        public void Reset()
        {
            Initialize();
        }

        // -----------------------------------------------------------------------
        // Private simulation steps
        // -----------------------------------------------------------------------

        private void MoveBall(float dt)
        {
            Ball.Position = Ball.Position + Ball.Velocity * dt;
        }

        private void HandleWallCollisions()
        {
            var pos = Ball.Position;
            var vel = Ball.Velocity;

            // Left wall
            if (pos.X - Ball.Radius < 0)
            {
                pos = new Vector2(Ball.Radius, pos.Y);
                vel = new Vector2(Math.Abs(vel.X), vel.Y);
            }
            // Right wall
            if (pos.X + Ball.Radius > Field.Width)
            {
                pos = new Vector2(Field.Width - Ball.Radius, pos.Y);
                vel = new Vector2(-Math.Abs(vel.X), vel.Y);
            }
            // Top wall
            if (pos.Y - Ball.Radius < 0)
            {
                pos = new Vector2(pos.X, Ball.Radius);
                vel = new Vector2(vel.X, Math.Abs(vel.Y));
            }

            Ball.Position = pos;
            Ball.Velocity = vel;

            // Bottom edge — ball is lost
            if (Ball.Position.Y - Ball.Radius > Field.Height)
            {
                State = GameState.Lost;
                OnBallLost?.Invoke();
            }
        }

        private void HandlePaddleCollision()
        {
            // Only check when ball is moving downward — prevents sticking
            if (Ball.Velocity.Y <= 0) return;

            if (CollisionDetector.CheckBallVsPaddle(Ball, Paddle, out Vector2 newVel))
            {
                Ball.Velocity = newVel;
                // Push ball above the paddle surface to avoid re-collision next frame
                Ball.Position = new Vector2(Ball.Position.X, Paddle.Top - Ball.Radius - 1f);
            }
        }

        private void HandleBrickCollisions()
        {
            foreach (var brick in _bricks)
            {
                if (!brick.IsAlive) continue;

                bool hit = CollisionDetector.CheckBallVsRect(
                    Ball,
                    brick.Left, brick.Top, brick.Right, brick.Bottom,
                    out bool reflectX, out bool reflectY);

                if (!hit) continue;

                // Reflect velocity
                var vel = Ball.Velocity;
                if (reflectX) vel = new Vector2(-vel.X,  vel.Y);
                if (reflectY) vel = new Vector2( vel.X, -vel.Y);
                Ball.Velocity = vel;

                // Apply damage
                brick.TakeDamage(Ball.Damage);

                if (!brick.IsAlive)
                {
                    Score += 10;
                    OnBrickDestroyed?.Invoke(brick);
                }

                // One brick collision per frame is enough
                break;
            }
        }

        private void CheckEndConditions()
        {
            if (State == GameState.Lost) return;

            foreach (var brick in _bricks)
                if (brick.IsAlive) return;   // at least one brick alive → not won yet

            State = GameState.Won;
            OnLevelComplete?.Invoke();
        }
    }
}
