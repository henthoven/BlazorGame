using System;
using System.Collections.Generic;

namespace GameEngine
{
    /// <summary>
    /// Represents a game state
    /// </summary>
    /// <typeparam name="TState">The state identifier</typeparam>
    public class GameState<TState>
    {
        /// <summary>
        /// The list of GameObjects that are used in this state
        /// </summary>
        public IEnumerable<GameObject<TState>> GameObjects { private set; get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gameObjects">The GameObjects that are part of this state</param>
        public GameState(IEnumerable<GameObject<TState>> gameObjects)
        {
            GameObjects = gameObjects;
        }
    }
}
