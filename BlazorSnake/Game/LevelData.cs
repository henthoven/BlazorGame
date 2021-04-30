using GameEngine;

namespace BlazorSnake.Game
{
    /// <summary>
    /// Contains the data of a level
    /// </summary>
    public class LevelData
    {
        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The number of apples a user must collect to complete the level
        /// </summary>
        public int AppleCount { get; set; }
        /// <summary>
        /// The time until a user can get bonus points
        /// </summary>
        public int TimeBonusUntil { get; set; }
        /// <summary>
        /// The X start position of the snake in this level
        /// </summary>
        public int StartXPosition { get; set; }
        /// <summary>
        /// The Y start position of the snake in this level
        /// </summary>
        public int StartYPosition { get; set; }
        /// <summary>
        /// The raw data that represents this level
        /// </summary>
        public int[][] Data { get; set; }

        /// <summary>
        /// Returns the start position of the snake as a Point
        /// </summary>
        public Point SnakeStartPosition => new Point { X = StartXPosition, Y = StartYPosition };

    }
}
