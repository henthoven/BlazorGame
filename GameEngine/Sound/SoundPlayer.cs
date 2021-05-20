using Howler.Blazor.Components;
using Howler.Blazor.Components.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Sound
{
    /// <summary>
    /// Class that wraps howl and can be used to play sounds
    /// </summary>
    public class SoundPlayer : ISoundPlayer
    {
        private IHowl _howl;
        private Dictionary<string, int> _sounds = new Dictionary<string, int>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="howl">The howl instance to use</param>
        public SoundPlayer(IHowl howl)
        {
            _howl = howl;
        }

        /// <summary>
        /// Plays the music from the specified url
        /// </summary>
        /// <param name="urlToPlay">The url of the sound to play</param>
        /// <param name="loop">Flag that indicates if the sound should loop</param>
        /// <returns>A completed task</returns>
        public async Task<int> Play(string urlToPlay, bool loop = false) 
        {
            return await _howl.Play(new HowlOptions { Sources = new string[] { urlToPlay }, Loop = loop });
        }

        /// <summary>
        /// Stops the sound with the given id
        /// </summary>
        /// <param name="soundId">The id of the sound to stop</param>
        /// <returns>A completed task</returns>
        public async Task Stop(int soundId)
        {
            await _howl.Stop(soundId); 
        }
    }
}
