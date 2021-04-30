using GameEngine;

namespace BlazorSnake
{
    /// <summary>
    /// Mapper class to map keycodes from KeyBoardEventArgs to GameEngine.KeyCode
    /// </summary>
    public static class InputMapper
    {
        /// <summary>
        /// Maps a keycode from KeyBoardEventArgs to a GameEngine.KeyCode
        /// </summary>
        /// <param name="key">The KeyBoardEventArgs code</param>
        /// <returns>The corrosponding GameEngine.KeyCode</returns>
        public static KeyCode Map(string key)
        {
            switch(key)
            {
                case "ArrowLeft":
                    return KeyCode.Left;
                case "ArrowRight":
                    return KeyCode.Right;
                case "ArrowUp":
                    return KeyCode.Up;
                case "ArrowDown":
                    return KeyCode.Down;
                case "Enter":
                    return KeyCode.Enter;
                case "KeyA":
                    return KeyCode.A;
                case "KeyZ":
                    return KeyCode.Z;
            }
            return KeyCode.Right;
        }
    }
}
