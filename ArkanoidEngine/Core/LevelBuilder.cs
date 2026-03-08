using ArkanoidEngine.Entities;
using System.Collections.Generic;

namespace ArkanoidEngine.Core
{
    /// <summary>
    /// Builds the brick layout for a level.
    /// Extend this class to add more levels — each method returns a brick list.
    /// </summary>
    public static class LevelBuilder
    {
        private const int   Columns    = 10;
        private const int   Rows       = 6;
        private const float BrickHeight = 20f;
        private const float GapX        = 3f;
        private const float GapY        = 5f;
        private const float OffsetY     = 70f;   // distance from top of field to first row
        private const float SideMargin  = 4f;

        /// <summary>
        /// Returns the brick layout for level 1.
        /// All bricks have 1 HP.
        /// </summary>
        public static List<Brick> BuildLevel1(int fieldWidth)
        {
            var bricks = new List<Brick>();

            float totalGapsX = GapX * (Columns - 1) + SideMargin * 2;
            float brickWidth = (fieldWidth - totalGapsX) / Columns;

            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    float x = SideMargin + col * (brickWidth + GapX);
                    float y = OffsetY    + row * (BrickHeight + GapY);

                    bricks.Add(new Brick(new Vector2(x, y), brickWidth, BrickHeight, hp: 1));
                }
            }

            return bricks;
        }
    }
}
