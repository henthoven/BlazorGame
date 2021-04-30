namespace GameEngine
{
    /// <summary>
    /// Interface for a GameTime object
    /// </summary>
    public interface IGameTime
    {
        /// <summary>
        /// The current FPS
        /// </summary>
        float Fps { get; }

        /// <summary>
        /// Sets the current timestamp
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        void SetTimeStamp(float timeStamp);
    }
}