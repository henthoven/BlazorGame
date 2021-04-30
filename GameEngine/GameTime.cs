using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    /// <summary>
    /// Represents the time the game is running. Used to calculate the FPS for example.
    /// </summary>
    public class GameTime : IGameTime
    {
        private float _fps = 0;
        private float _lastTimeStamp = 0;
        private float _elapsedTime = 0;

        /// <summary>
        /// The current FPS
        /// </summary>
        public float Fps => _fps;

        /// <summary>
        /// Sets the timestamp to the given timestamp
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        public void SetTimeStamp(float timeStamp)
        {
            _elapsedTime = timeStamp - _lastTimeStamp;
            _lastTimeStamp = timeStamp;
            _fps = 1000f / _elapsedTime;
        }
    }
}
