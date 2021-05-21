namespace GameEngine
{
    /// <summary>
    /// Interface for an InputManager
    /// </summary>
    public interface IInputManager
    {
        /// <summary>
        /// Returns if the given keycode is currently pressed
        /// </summary>
        /// <param name="keyCode">The KeyCode to check</param>
        bool IsPressed(KeyCode keyCode);
        /// <summary>
        /// Returns if the given KeyCode was the last pressed key
        /// </summary>
        /// <param name="keyCode">The KeyCode to check</param>
        bool IsLastPressed(KeyCode keyCode);
        /// <summary>
        /// Sets the given KeyCode as pressed key
        /// </summary>
        /// <param name="keyCode">The KeyCode of the key that is pressed</param>
        void SetKeyPressed(KeyCode keyCode);
        /// <summary>
        /// Sets the given KeyCode as released key
        /// </summary>
        /// <param name="keyCode">The KeyCode of the key that is released</param>
        void SetKeyReleased(KeyCode keyCode);
        /// <summary>
        /// Returns the last pressed key
        /// </summary>
        KeyCode LastPressedKey { get; }
        /// <summary>
        /// Resets the last pressed key
        /// </summary>
        void ResetLastPressedKey();

        void SetTouchEnter(double xPosition, double yPosition);

        void SetTouchEnd(double xPosition, double yPosition);
    }
}