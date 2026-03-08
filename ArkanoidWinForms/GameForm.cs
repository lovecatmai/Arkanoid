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
            ClientSize      = new Size(600, 800);
            Text            = "Arkanoid";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox     = false;
            DoubleBuffered  = true;
            BackColor       = Color.Black;
            Cursor.Hide();
        }

        private void SetupGame()
        {
            gameEngine   = new GameEngine(fieldWidth: 600, fieldHeight: 800);
            gameRenderer = new Renderer(gameEngine);

            gameEngine.OnBallLost      += () => { };
            gameEngine.OnLevelComplete += () => { };

            gameLoopTimer          = new Timer { Interval = 16 };
            gameLoopTimer.Tick    += OnGameLoopTick;
            lastFrameTime          = DateTime.UtcNow;
            gameLoopTimer.Start();

            MouseMove += OnMouseMove;
            MouseDown += OnMouseDown;
            KeyDown   += OnKeyDown;
        }

        private void OnGameLoopTick(object sender, EventArgs e)
        {
            DateTime currentFrameTime = DateTime.UtcNow;
            float    deltaTime        = (float)(currentFrameTime - lastFrameTime).TotalSeconds;
            lastFrameTime             = currentFrameTime;

            deltaTime = Math.Min(deltaTime, 0.05f);

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
