using System;
using System.Drawing;
using System.Windows.Forms;
using ArkanoidEngine.Core;

namespace ArkanoidWinForms
{

    /// <summary>
    /// Главная форма — управляет окном, вводом и игровым циклом.
    /// <summary>
    public sealed class GameForm : Form
    {
        private GameEngine gameEngine;
        private Renderer   gameRenderer;
        private Timer      gameLoopTimer;
        private DateTime   lastFrameTime;

        private const float maxAllowedDeltaTimeInSeconds = 0.05f;

        private int ClientSizeWindowWidth = 600;
        private int ClientSizeWindowHeight = 800;
        private int gameLoopTimerInterval = 16;

        /// <summary>
        /// Создаёт форму и запускает игру.
        /// </summary>
        public GameForm()
        {
            SetupForm();
            SetupGame();
        }
        
        private void SetupForm()
        {
            ClientSize      = new Size(ClientSizeWindowWidth, ClientSizeWindowHeight);
            Text            = "Arkanoid";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox     = false;
            DoubleBuffered  = true;
            BackColor       = Color.Black;
            Cursor.Hide();
        }

        private void SetupGame()
        {
            gameEngine   = new GameEngine(fieldWidth: ClientSizeWindowWidth, fieldHeight: ClientSizeWindowHeight);
            gameRenderer = new Renderer(gameEngine);

            gameEngine.OnBallLost      += () => { };
            gameEngine.OnLevelComplete += () => { };

            gameLoopTimer          = new Timer { Interval = gameLoopTimerInterval };
            gameLoopTimer.Tick    += OnGameLoopTick;
            lastFrameTime          = DateTime.UtcNow;
            gameLoopTimer.Start();

            MouseMove += OnMouseMove;
            MouseDown += OnMouseDown;
            KeyDown   += OnKeyDown;
        }

        private void OnGameLoopTick(object sender, EventArgs e)
        {
            var currentFrameTime = DateTime.UtcNow;
            var deltaTime        = (float)(currentFrameTime - lastFrameTime).TotalSeconds;
            lastFrameTime             = currentFrameTime;

            deltaTime = Math.Min(deltaTime, maxAllowedDeltaTimeInSeconds);

            gameEngine.Update(deltaTime);
            Invalidate();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            gameEngine.SetPaddleX(e.X);
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            HandleLaunchOrRestart();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                HandleLaunchOrRestart();
        }

        private void HandleLaunchOrRestart()
        {
            switch (gameEngine.State)
            {
                case GameState.WaitingToStart:
                    gameEngine.StartGame();
                    break;
                case GameState.Won:
                case GameState.Lost:
                    gameEngine.Reset();
                    break;
            }
        }

        /// <summary>
        /// Передаёт контекст Graphics рендереру для отрисовки текущего кадра.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gameRenderer.Render(e.Graphics);
        }
    }
}
