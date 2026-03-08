using ArkanoidEngine.Entities;
using System.Collections.Generic;

namespace ArkanoidEngine.Core
{
    /// <summary>
    /// Генерирует расстановку кирпичей для уровней.
    /// </summary>
    public static class LevelBuilder
    {
        private const int   numberOfColumns          = 10;
        private const int   numberOfRows             = 6;
        private const float brickHeight              = 20f;
        private const float horizontalGapBetweenBricks = 3f;
        private const float verticalGapBetweenBricks   = 5f;
        private const float topOffsetBeforeFirstRow  = 70f;
        private const float sideMarginFromWall       = 4f;

        /// <summary>
        /// Строит список кирпичей для первого уровня.
        /// </summary>
        public static List<Brick> BuildLevel1(int fieldWidth)
        {
            var bricks = new List<Brick>();

            float totalHorizontalGaps = horizontalGapBetweenBricks * (numberOfColumns - 1) + sideMarginFromWall * 2;
            float brickWidth          = (fieldWidth - totalHorizontalGaps) / numberOfColumns;

            for (int row = 0; row < numberOfRows; row++)
            {
                for (int col = 0; col < numberOfColumns; col++)
                {
                    float x = sideMarginFromWall + col * (brickWidth + horizontalGapBetweenBricks);
                    float y = topOffsetBeforeFirstRow + row * (brickHeight + verticalGapBetweenBricks);

                    bricks.Add(new Brick(new Vector2(x, y), brickWidth, brickHeight, hp: 1));
                }
            }

            return bricks;
        }
    }
}
