namespace ArkanoidEngine.Core
{
    /// <summary>
    /// Перечисление возможных состояний игры.
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// Мяч на платформе, ожидание запуска.
        /// </summary>
        WaitingToStart,

        /// <summary>
        /// Игра идёт, мяч в движении.
        /// </summary>
        Playing,

        /// <summary>
        /// Все кирпичи уничтожены — победа.
        /// </summary>
        Won,

        /// <summary>
        /// Мяч упал за нижний край — поражение.
        /// </summary>
        Lost
    }
}
