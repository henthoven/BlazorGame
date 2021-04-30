namespace GameEngine
{
    /// <summary>
    /// Interface for a GameTimer
    /// </summary>
    public interface IGameTimer
    {
        /// <summary>
        /// Flag that indicates if the timer is started
        /// </summary>
        bool IsStarted { get; }
        /// <summary>
        /// Returns the time that has been elapsed since the timer started in seconds
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        /// <returns></returns>
        int GetElapsedTimeInSeconds(float gameTime);
        /// <summary>
        /// Starts the timer
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        void Start(float gameTime);
        /// <summary>
        /// Stops the timer
        /// </summary>
        /// <param name="gameTime">The current gametime</param>
        void Stop(float gameTime);
        /// <summary>
        /// Resets the timer
        /// </summary>
        void Reset();
    }
}