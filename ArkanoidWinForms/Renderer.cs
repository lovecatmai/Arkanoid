using System.Drawing;
using ArkanoidEngine.Core;

namespace ArkanoidWinForms
{
    /// <summary>
    /// Handles all GDI+ rendering.
    /// Knows nothing about WinForms controls — only receives a Graphics object.
    /// </summary>
    internal sealed class Renderer
    {
        private readonly GameEngine _engine;

        // Pre-allocated brushes / pens / fonts to avoid per-frame allocations
        private static readonly Font       FontHud      = new Font("Consolas", 14, FontStyle.Bold);
        private static readonly Font       FontBig      = new Font("Consolas", 32, FontStyle.Bold);
        private static readonly Font       FontSub      = new Font("Consolas", 14, FontStyle.Regular);
        private static readonly SolidBrush BrushOverlay = new SolidBrush(Color.FromArgb(170, 10, 10, 20));
        private static readonly SolidBrush BrushBrick   = new SolidBrush(Color.FromArgb(255, 72, 149, 239));
        private static readonly SolidBrush BrushBrickHi = new SolidBrush(Color.FromArgb(255, 140, 195, 255));
        private static readonly Pen        PenBrick      = new Pen(Color.FromArgb(255, 30,  90, 180), 1f);
        private static readonly SolidBrush BrushPaddle  = new SolidBrush(Color.White);
        private static readonly SolidBrush BrushBall    = new SolidBrush(Color.FromArgb(255, 255, 160, 40));
        private static readonly SolidBrush BrushText    = new SolidBrush(Color.White);
        private static readonly SolidBrush BrushSubText = new SolidBrush(Color.FromArgb(200, 200, 200, 200));

        public Renderer(GameEngine engine)
        {
            _engine = engine;
        }

        public void Render(Graphics g)
        {
            g.Clear(Color.FromArgb(10, 10, 20));   // dark navy background

            DrawBricks(g);
            DrawPaddle(g);
            DrawBall(g);
            DrawHud(g);

            if (_engine.State != ArkanoidEngine.Core.GameState.Playing)
                DrawOverlay(g);
        }

        // -----------------------------------------------------------------------
        private void DrawBricks(Graphics g)
        {
            foreach (var brick in _engine.Bricks)
            {
                if (!brick.IsAlive) continue;

                var fill = new RectangleF(brick.Left, brick.Top, brick.Width, brick.Height);

                // Main fill
                g.FillRectangle(BrushBrick, fill);

                // Highlight strip along the top
                g.FillRectangle(BrushBrickHi,
                    new RectangleF(fill.X + 2, fill.Y + 2, fill.Width - 4, 4));

                // Border
                g.DrawRectangle(PenBrick, fill.X, fill.Y, fill.Width, fill.Height);
            }
        }

        private void DrawPaddle(Graphics g)
        {
            var p = _engine.Paddle;
            // Rounded-looking paddle via two rects
            var rect = new RectangleF(p.Left, p.Top, p.Width, p.Height);
            g.FillRectangle(BrushPaddle, rect);
        }

        private void DrawBall(Graphics g)
        {
            var b = _engine.Ball;
            var rect = new RectangleF(
                b.Position.X - b.Radius,
                b.Position.Y - b.Radius,
                b.Radius * 2,
                b.Radius * 2);
            g.FillEllipse(BrushBall, rect);
        }

        private void DrawHud(Graphics g)
        {
            g.DrawString($"SCORE  {_engine.Score:D5}", FontHud, BrushText, 10, 12);
        }

        private void DrawOverlay(Graphics g)
        {
            int w = _engine.Field.Width;
            int h = _engine.Field.Height;

            // Dark veil
            g.FillRectangle(BrushOverlay, 0, 0, w, h);

            string title, sub;
            switch (_engine.State)
            {
                case ArkanoidEngine.Core.GameState.WaitingToStart:
                    title = "ARKANOID";
                    sub   = "Move mouse · Click or Space to launch";
                    break;
                case ArkanoidEngine.Core.GameState.Won:
                    title = "YOU WIN!";
                    sub   = $"Score: {_engine.Score}   ·   Click or Space to restart";
                    break;
                default: // Lost
                    title = "GAME OVER";
                    sub   = $"Score: {_engine.Score}   ·   Click or Space to restart";
                    break;
            }

            DrawCenteredString(g, title, FontBig, BrushText,    w, h / 2f - 50);
            DrawCenteredString(g, sub,   FontSub,  BrushSubText, w, h / 2f + 10);
        }

        private static void DrawCenteredString(Graphics g, string text, Font font, Brush brush, float fieldWidth, float y)
        {
            var size = g.MeasureString(text, font);
            g.DrawString(text, font, brush, (fieldWidth - size.Width) / 2f, y);
        }
    }
}
