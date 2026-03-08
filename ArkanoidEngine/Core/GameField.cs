namespace ArkanoidEngine.Core
{
    /// <summary>
    /// Defines the dimensions of the game area.
    /// Origin (0,0) is top-left. X grows right, Y grows down.
    /// </summary>
    public class GameField
    {
        public int Width  { get; }
        public int Height { get; }

        public GameField(int width, int height)
        {
            Width  = width;
            Height = height;
        }
    }
}
