using System;
using System.Drawing;
using System.Windows.Forms;
using ArkanoidEngine.Core;

namespace ArkanoidWinForms
{
    /// <summary>
    /// Главная форма — управляет окном, вводом и игровым циклом.
    /// </summary>
    public sealed class GameForm : Form
    {
        private GameEngine gameEngine;
        private Renderer gameRenderer;
        private Timer gameLoopTimer;
        private DateTime lastFrameTime;
        private Bitmap frameBuffer;
        private Graphics frameBufferGraphics;

        private const string windowTitle = "Arkanoid";
        private const float maxAllowedDeltaTimeInSeconds = 0.05f;
        private const int clientSizeWindowWidth = 600;
        private const int clientSizeWindowHeight = 800;
        private const int gameLoopTimerInterval = 16;
        private const int frameBufferDrawPositionX = 0;
        private const int frameBufferDrawPositionY = 0;

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
            ClientSize = new Size(clientSizeWindowWidth, clientSizeWindowHeight);
            Text = windowTitle;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.Black;
            Cursor.Hide();
        }

        private void SetupGame()
        {
            gameEngine = new GameEngine(fieldWidth: clientSizeWindowWidth, fieldHeight: clientSizeWindowHeight);
            gameRenderer = new Renderer(gameEngine);

            gameLoopTimer = new Timer { Interval = gameLoopTimerInterval };
            gameLoopTimer.Tick += OnGameLoopTick;
            lastFrameTime = DateTime.UtcNow;

            Load += OnFormLoad;
            MouseMove += OnMouseMove;
            MouseDown += OnMouseDown;
            KeyDown += OnKeyDown;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            frameBuffer = new Bitmap(ClientSize.Width, ClientSize.Height);
            frameBufferGraphics = Graphics.FromImage(frameBuffer);
            gameLoopTimer.Start();
        }

        private void OnGameLoopTick(object sender, EventArgs e)
        {
            var currentFrameTime = DateTime.UtcNow;
            var deltaTime = (float)(currentFrameTime - lastFrameTime).TotalSeconds;
            lastFrameTime = currentFrameTime;

            deltaTime = Math.Min(deltaTime, maxAllowedDeltaTimeInSeconds);

            gameEngine.Update(deltaTime);
            RenderFrame();
        }

        private void RenderFrame()
        {
            if (frameBufferGraphics == null) return;

            frameBufferGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gameRenderer.Render(frameBufferGraphics);

            using (var screenGraphics = CreateGraphics())
            {
                screenGraphics.DrawImage(frameBuffer, frameBufferDrawPositionX, frameBufferDrawPositionY);
            }
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
            {
                HandleLaunchOrRestart();
            }
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
    }
}