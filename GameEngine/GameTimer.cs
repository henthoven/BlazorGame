using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    /// <summary>
    /// A game timer. Used to check and display times in a game.
    /// </summary>
    public class GameTimer : IGameTimer
    {
        private float _startGameTime;
        private float? _endGameTime;

        /// <summary>
        /// Indicates if the timer is started
        /// </summary>
        public bool IsStarted { get; private set; }

        /// <summary>
        /// Starts the timer
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public void Start(float gameTime)
        {
            _startGameTime = gameTime;
            IsStarted = true;
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        /// <param name="gameTime">The current gametime</param>
        public void Stop(float gameTime)
        {
            _endGameTime = gameTime;
        }

        /// <summary>
        /// Resets the timer
        /// </summary>
        public void Reset()
        {
            IsStarted = false;
            _endGameTime = null;
        }

        /// <summary>
        /// Returns the time that has been elapsed since the timer started in seconds
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        /// <returns></returns>
        public int GetElapsedTimeInSeconds(float gameTime)
        {
            if (IsStarted)
            {
                float timeToUse = gameTime;
                if (_endGameTime.HasValue)
                    timeToUse = _endGameTime.Value;
                return (int)((timeToUse - _startGameTime) / 1000f);
            }
            return 0;
        }
    }
}
