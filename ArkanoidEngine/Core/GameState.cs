namespace ArkanoidEngine.Core
{
    public enum GameState
    {
        WaitingToStart, // Ball is on paddle, waiting for player to launch
        Playing,
        Won,
        Lost
    }
}
