using ArkanoidEngine.Entities;
using System.Collections.Generic;
using static ArkanoidEngine.GameConstants;

namespace ArkanoidEngine.Core
{
    /// <summary>
    /// Генерирует расстановку кирпичей для уровней.
    /// </summary>
    public static class LevelBuilder
    {
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