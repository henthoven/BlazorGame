namespace BlazorSnake.Game
{
    /// <summary>
    /// The states that the game can be in
    /// </summary>
    public enum SnakeGameState
    {
        /// <summary>
        /// The mainmenu is shown
        /// </summary>
        MainMenu,
        /// <summary>
        /// LevelStart message should be shown
        /// </summary>
        LevelStart,
        /// <summary>
        /// Level is running, the player is playing
        /// </summary>
        LevelRunning,
        /// <summary>
        /// The level is completed
        /// </summary>
        LevelComplete,
        /// <summary>
        /// The game is over
        /// </summary>
        GameOver
    }
}
