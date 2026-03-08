using ArkanoidEngine.Entities;
using System.Collections.Generic;

namespace ArkanoidEngine.Core
{
    /// <summary>
    /// Генерирует расстановку кирпичей для уровней.
    /// </summary>
    public static class LevelBuilder
    {
        private const int numberOfColumns = 10;
        private const int numberOfRows = 6;
        private const float brickHeightInPixels = 20f;
        private const float horizontalGapBetweenBricks = 3f;
        private const float verticalGapBetweenBricks = 5f;
        private const float topOffsetBeforeFirstRow = 70f;
        private const float sideMarginFromWall = 4f;
        private const int numberOfGapsBetweenColumns = numberOfColumns - 1;
        private const int numberOfSideMargins = 2;
        private const int brickHitPointsForLevel1 = 1;
        private const int firstColumnIndex = 0;
        private const int firstRowIndex = 0;

        /// <summary>
        /// Строит список кирпичей для первого уровня.
        /// </summary>
        public static List<Brick> BuildLevel1(int fieldWidth)
        {
            var bricks = new List<Brick>();

            var totalHorizontalGaps = horizontalGapBetweenBricks * numberOfGapsBetweenColumns + sideMarginFromWall * numberOfSideMargins;
            var brickWidth = (fieldWidth - totalHorizontalGaps) / numberOfColumns;

            for (var row = firstRowIndex; row < numberOfRows; row++)
            {
                for (var col = firstColumnIndex; col < numberOfColumns; col++)
                {
                    var x = sideMarginFromWall + col * (brickWidth + horizontalGapBetweenBricks);
                    var y = topOffsetBeforeFirstRow + row * (brickHeightInPixels + verticalGapBetweenBricks);

                    bricks.Add(new Brick(new Vector2(x, y), brickWidth, brickHeightInPixels, hp: brickHitPointsForLevel1));
                }
            }

            return bricks;
        }
    }
}