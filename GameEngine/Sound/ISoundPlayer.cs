using System.Threading.Tasks;

namespace GameEngine.Sound
{
    /// <summary>
    /// Interface for a soundplayer
    /// </summary>
    public interface ISoundPlayer
    {
        /// <summary>
        /// Plays the music from the specified url
        /// </summary>
        /// <param name="urlToPlay">The url of the sound to play</param>
        /// <param name="loop">Flag that indicates if the sound should loop</param>
        /// <returns>A completed task</returns>
        Task<int> Play(string urlToPlay, bool loop = false);

        /// <summary>
        /// Stops the sound with the given id
        /// </summary>
        /// <param name="soundId">The id of the sound to stop</param>
        /// <returns>A completed task</returns>
        Task Stop(int soundId);
    }
}