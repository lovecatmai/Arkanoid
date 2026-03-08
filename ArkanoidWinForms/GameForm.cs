using System;
using System.Drawing;
using System.Windows.Forms;
using ArkanoidEngine.Core;

namespace ArkanoidWinForms
{
    /// <summary>
    /// WinForms shell. Responsibilities:
    ///   - Host the game window
    ///   - Drive the game loop via a Timer
    ///   - Forward mouse/keyboard input to the engine
    ///   - Hand a Graphics context to the Renderer each frame
    ///
    /// All game logic lives in ArkanoidEngine — this form is intentionally thin.
    /// </summary>
    public sealed class GameForm : Form
    {
        private GameEngine _engine;
        private Renderer   _renderer;
        private Timer      _gameTimer;
        private DateTime   _lastTick;

        public GameForm()
        {
            SetupForm();
            SetupGame();
        }

        // -----------------------------------------------------------------------
        private void SetupForm()
        {
            ClientSize        = new Size(600, 800);
            Text              = "Arkanoid";
            FormBorderStyle   = FormBorderStyle.FixedSingle;
            MaximizeBox       = false;
            DoubleBuffered    = true;   // eliminates flicker
            BackColor         = Color.Black;
            Cursor.Hide();
        }

        private void SetupGame()
        {
            _engine   = new GameEngine(fieldWidth: 600, fieldHeight: 800);
            _renderer = new Renderer(_engine);

            _engine.OnBallLost      += () => { /* Could trigger screen shake, etc. */ };
            _engine.OnLevelComplete += () => { /* Could trigger fanfare, etc. */ };

            _gameTimer          = new Timer { Interval = 16 }; // ~62 fps
            _gameTimer.Tick     += GameLoop;
            _lastTick           = DateTime.UtcNow;
            _gameTimer.Start();

            MouseMove  += OnMouseMove;
            MouseDown  += OnMouseDown;
            KeyDown    += OnKeyDown;
        }

        // -----------------------------------------------------------------------
        // Game loop
        // -----------------------------------------------------------------------
        private void GameLoop(object sender, EventArgs e)
        {
            DateTime now   = DateTime.UtcNow;
            float    delta = (float)(now - _lastTick).TotalSeconds;
            _lastTick      = now;

            // Cap delta to avoid spiral of death after window drag / breakpoint
            delta = Math.Min(delta, 0.05f);

            _engine.Update(delta);
            Invalidate();
        }

        // -----------------------------------------------------------------------
        // Input forwarding
        // -----------------------------------------------------------------------
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            _engine.SetPaddleX(e.X);
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
            switch (_engine.State)
            {
                case GameState.WaitingToStart:
                    _engine.StartGame();
                    break;
                case GameState.Won:
                case GameState.Lost:
                    _engine.Reset();
                    break;
            }
        }

        // -----------------------------------------------------------------------
        // Rendering
        // -----------------------------------------------------------------------
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            _renderer.Render(e.Graphics);
        }
    }
}
