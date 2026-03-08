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

        private const string hudFontFamily = "Consolas";
        private const float hudFontSize = 14f;
        private const float overlayTitleFontSize = 32f;
        private const float overlaySubtitleFontSize = 14f;

        private const int fullyOpaqueAlpha = 255;
        private const int halvingDivisor = 2;
        private const float halvingDivisorFloat = 2f;
        private const int bothSidesMultiplier = 2;

        private const int overlayBackgroundAlpha = 170;
        private const int overlayBackgroundRed = 10;
        private const int overlayBackgroundGreen = 10;
        private const int overlayBackgroundBlue = 20;

        private const int brickFillRed = 72;
        private const int brickFillGreen = 149;
        private const int brickFillBlue = 239;

        private const int brickHighlightRed = 140;
        private const int brickHighlightGreen = 195;
        private const int brickHighlightBlue = 255;

        private const int brickBorderRed = 30;
        private const int brickBorderGreen = 90;
        private const int brickBorderBlue = 180;
        private const float brickBorderPenWidth = 1f;

        private const int ballFillRed = 255;
        private const int ballFillGreen = 160;
        private const int ballFillBlue = 40;

        private const int secondaryTextAlpha = 200;
        private const int secondaryTextRed = 200;
        private const int secondaryTextGreen = 200;
        private const int secondaryTextBlue = 200;

        private const int backgroundRed = 10;
        private const int backgroundGreen = 10;
        private const int backgroundBlue = 20;

        private const float brickHighlightOffsetFromEdge = 2f;
        private const float brickHighlightStripHeight = 4f;

        private const float hudScorePositionX = 10f;
        private const float hudScorePositionY = 12f;

        private const float overlayTitleOffsetAboveCenter = 50f;
        private const float overlaySubtitleOffsetBelowCenter = 10f;

        private const int overlayStartPositionX = 0;
        private const int overlayStartPositionY = 0;

        private static readonly Font hudFont = new Font(hudFontFamily, hudFontSize, FontStyle.Bold);
        private static readonly Font overlayTitleFont = new Font(hudFontFamily, overlayTitleFontSize, FontStyle.Bold);
        private static readonly Font overlaySubtitleFont = new Font(hudFontFamily, overlaySubtitleFontSize, FontStyle.Regular);
        private static readonly SolidBrush overlayBackgroundBrush = new SolidBrush(Color.FromArgb(overlayBackgroundAlpha, overlayBackgroundRed, overlayBackgroundGreen, overlayBackgroundBlue));
        private static readonly SolidBrush brickFillBrush = new SolidBrush(Color.FromArgb(fullyOpaqueAlpha, brickFillRed, brickFillGreen, brickFillBlue));
        private static readonly SolidBrush brickHighlightBrush = new SolidBrush(Color.FromArgb(fullyOpaqueAlpha, brickHighlightRed, brickHighlightGreen, brickHighlightBlue));
        private static readonly Pen brickBorderPen = new Pen(Color.FromArgb(fullyOpaqueAlpha, brickBorderRed, brickBorderGreen, brickBorderBlue), brickBorderPenWidth);
        private static readonly SolidBrush paddleFillBrush = new SolidBrush(Color.White);
        private static readonly SolidBrush ballFillBrush = new SolidBrush(Color.FromArgb(fullyOpaqueAlpha, ballFillRed, ballFillGreen, ballFillBlue));
        private static readonly SolidBrush primaryTextBrush = new SolidBrush(Color.White);
        private static readonly SolidBrush secondaryTextBrush = new SolidBrush(Color.FromArgb(secondaryTextAlpha, secondaryTextRed, secondaryTextGreen, secondaryTextBlue));

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
            graphics.Clear(Color.FromArgb(backgroundRed, backgroundGreen, backgroundBlue));

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
                    new RectangleF(
                        brickRect.X + brickHighlightOffsetFromEdge,
                        brickRect.Y + brickHighlightOffsetFromEdge,
                        brickRect.Width - brickHighlightOffsetFromEdge * bothSidesMultiplier,
                        brickHighlightStripHeight));
                graphics.DrawRectangle(brickBorderPen, brickRect.X, brickRect.Y, brickRect.Width, brickRect.Height);
            }
        }

        private void DrawPaddle(Graphics graphics)
        {
            var paddle = gameEngine.Paddle;
            var paddleRect = new RectangleF(paddle.Left, paddle.Top, paddle.Width, paddle.Height);
            graphics.FillRectangle(paddleFillBrush, paddleRect);
        }

        private void DrawBall(Graphics graphics)
        {
            var ball = gameEngine.Ball;
            var ballRect = new RectangleF(
                ball.Position.X - ball.Radius,
                ball.Position.Y - ball.Radius,
                ball.Radius * bothSidesMultiplier,
                ball.Radius * bothSidesMultiplier);
            graphics.FillEllipse(ballFillBrush, ballRect);
        }

        private void DrawHud(Graphics graphics)
        {
            graphics.DrawString($"SCORE  {gameEngine.Score:D5}", hudFont, primaryTextBrush, hudScorePositionX, hudScorePositionY);
        }

        private void DrawOverlay(Graphics graphics)
        {
            var fieldWidth = gameEngine.Field.Width;
            var fieldHeight = gameEngine.Field.Height;

            graphics.FillRectangle(overlayBackgroundBrush, overlayStartPositionX, overlayStartPositionY, fieldWidth, fieldHeight);

            string titleText, subtitleText;
            switch (gameEngine.State)
            {
                case GameState.WaitingToStart:
                    titleText = "ARKANOID";
                    subtitleText = "Move mouse · Click or Space to launch";
                    break;
                case GameState.Won:
                    titleText = "YOU WIN!";
                    subtitleText = $"Score: {gameEngine.Score}   ·   Click or Space to restart";
                    break;
                default:
                    titleText = "GAME OVER";
                    subtitleText = $"Score: {gameEngine.Score}   ·   Click or Space to restart";
                    break;
            }

            DrawStringCenteredHorizontally(graphics, titleText, overlayTitleFont, primaryTextBrush, fieldWidth, fieldHeight / halvingDivisorFloat - overlayTitleOffsetAboveCenter);
            DrawStringCenteredHorizontally(graphics, subtitleText, overlaySubtitleFont, secondaryTextBrush, fieldWidth, fieldHeight / halvingDivisorFloat + overlaySubtitleOffsetBelowCenter);
        }

        private static void DrawStringCenteredHorizontally(Graphics graphics, string text, Font font, Brush brush, float fieldWidth, float positionY)
        {
            var textSize = graphics.MeasureString(text, font);
            graphics.DrawString(text, font, brush, (fieldWidth - textSize.Width) / halvingDivisorFloat, positionY);
        }
    }
}