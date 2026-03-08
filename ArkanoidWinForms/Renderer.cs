using System.Drawing;
using ArkanoidEngine.Core;

namespace ArkanoidWinForms
{
    /// <summary>
    /// Отрисовка всех игровых объектов через GDI+ на переданном контексте Graphics.
    /// </summary>
    internal sealed class Renderer
    {
        private readonly GameEngine gameEngine;

        private static readonly Font       hudFont             = new Font("Consolas", 14, FontStyle.Bold);
        private static readonly Font       overlayTitleFont    = new Font("Consolas", 32, FontStyle.Bold);
        private static readonly Font       overlaySubtitleFont = new Font("Consolas", 14, FontStyle.Regular);
        private static readonly SolidBrush overlayBackgroundBrush = new SolidBrush(Color.FromArgb(170, 10, 10, 20));
        private static readonly SolidBrush brickFillBrush     = new SolidBrush(Color.FromArgb(255, 72, 149, 239));
        private static readonly SolidBrush brickHighlightBrush = new SolidBrush(Color.FromArgb(255, 140, 195, 255));
        private static readonly Pen        brickBorderPen     = new Pen(Color.FromArgb(255, 30, 90, 180), 1f);
        private static readonly SolidBrush paddleFillBrush    = new SolidBrush(Color.White);
        private static readonly SolidBrush ballFillBrush      = new SolidBrush(Color.FromArgb(255, 255, 160, 40));
        private static readonly SolidBrush primaryTextBrush   = new SolidBrush(Color.White);
        private static readonly SolidBrush secondaryTextBrush = new SolidBrush(Color.FromArgb(200, 200, 200, 200));

        /// <summary>
        /// Создаёт рендерер, привязанный к указанному движку.
        /// </summary>
        public Renderer(GameEngine gameEngine)
        {
            this.gameEngine = gameEngine;
        }

        /// <summary>
        /// Рисует полный кадр на переданном контексте Graphics.
        /// </summary>
        public void Render(Graphics graphics)
        {
            graphics.Clear(Color.FromArgb(10, 10, 20));

            DrawBricks(graphics);
            DrawPaddle(graphics);
            DrawBall(graphics);
            DrawHud(graphics);

            if (gameEngine.State != GameState.Playing)
                DrawOverlay(graphics);
        }

        private void DrawBricks(Graphics graphics)
        {
            foreach (var brick in gameEngine.Bricks)
            {
                if (!brick.IsAlive) continue;

                var brickRect = new RectangleF(brick.Left, brick.Top, brick.Width, brick.Height);

                graphics.FillRectangle(brickFillBrush, brickRect);
                graphics.FillRectangle(brickHighlightBrush,
                    new RectangleF(brickRect.X + 2, brickRect.Y + 2, brickRect.Width - 4, 4));
                graphics.DrawRectangle(brickBorderPen, brickRect.X, brickRect.Y, brickRect.Width, brickRect.Height);
            }
        }

        private void DrawPaddle(Graphics graphics)
        {
            var paddle     = gameEngine.Paddle;
            var paddleRect = new RectangleF(paddle.Left, paddle.Top, paddle.Width, paddle.Height);
            graphics.FillRectangle(paddleFillBrush, paddleRect);
        }

        private void DrawBall(Graphics graphics)
        {
            var ball     = gameEngine.Ball;
            var ballRect = new RectangleF(
                ball.Position.X - ball.Radius,
                ball.Position.Y - ball.Radius,
                ball.Radius * 2,
                ball.Radius * 2);
            graphics.FillEllipse(ballFillBrush, ballRect);
        }

        private void DrawHud(Graphics graphics)
        {
            graphics.DrawString($"SCORE  {gameEngine.Score:D5}", hudFont, primaryTextBrush, 10, 12);
        }

        private void DrawOverlay(Graphics graphics)
        {
            int fieldWidth  = gameEngine.Field.Width;
            int fieldHeight = gameEngine.Field.Height;

            graphics.FillRectangle(overlayBackgroundBrush, 0, 0, fieldWidth, fieldHeight);

            string titleText, subtitleText;
            switch (gameEngine.State)
            {
                case GameState.WaitingToStart:
                    titleText    = "ARKANOID";
                    subtitleText = "Move mouse · Click or Space to launch";
                    break;
                case GameState.Won:
                    titleText    = "YOU WIN!";
                    subtitleText = $"Score: {gameEngine.Score}   ·   Click or Space to restart";
                    break;
                default:
                    titleText    = "GAME OVER";
                    subtitleText = $"Score: {gameEngine.Score}   ·   Click or Space to restart";
                    break;
            }

            DrawStringCenteredHorizontally(graphics, titleText,    overlayTitleFont,    primaryTextBrush,   fieldWidth, fieldHeight / 2f - 50);
            DrawStringCenteredHorizontally(graphics, subtitleText, overlaySubtitleFont, secondaryTextBrush, fieldWidth, fieldHeight / 2f + 10);
        }

        private static void DrawStringCenteredHorizontally(Graphics graphics, string text, Font font, Brush brush, float fieldWidth, float positionY)
        {
            var textSize = graphics.MeasureString(text, font);
            graphics.DrawString(text, font, brush, (fieldWidth - textSize.Width) / 2f, positionY);
        }
    }
}
