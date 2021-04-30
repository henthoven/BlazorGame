using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    /// <summary>
    /// Class to handle input
    /// </summary>
    public class InputManager : IInputManager
    {
        private List<KeyCode> _pressedKeys = new List<KeyCode>();
        private KeyCode _lastPressedKey = KeyCode.Right;

        /// <inheritdoc/>
        public void SetKeyPressed(KeyCode keyCode)
        {
            if (!_pressedKeys.Contains(keyCode))
                _pressedKeys.Add(keyCode);
            _lastPressedKey = keyCode;
        }
        /// <inheritdoc/>
        public void SetKeyReleased(KeyCode keyCode)
        {
            if (_pressedKeys.Contains(keyCode))
                _pressedKeys.Remove(keyCode);

        }
        /// <inheritdoc/>
        public void ResetLastPressedKey()
        {
            _lastPressedKey = KeyCode.None;
        }
        /// <inheritdoc/>
        public bool IsPressed(KeyCode keyCode) => _pressedKeys.Contains(keyCode);
        
        /// <inheritdoc/>
        public bool IsLastPressed(KeyCode keyCode) =>_lastPressedKey == keyCode;

        /// <inheritdoc/>
        public KeyCode LastPressedKey => _lastPressedKey;
    }
}
