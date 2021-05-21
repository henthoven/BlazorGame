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

        private Point? _touchStart;

        public void SetTouchEnter(double xPosition, double yPosition)
        {
            _touchStart = new Point(xPosition, yPosition);
        }

        public void SetTouchEnd(double xPosition, double yPosition)
        {
            if (_touchStart != null)
            {
                double xDifference = _touchStart.Value.X - xPosition;
                double yDifference = _touchStart.Value.Y - yPosition;
                if (Math.Abs(xDifference) > Math.Abs(yDifference)) // move over x axis
                {
                    if (xDifference > 100)
                        _lastPressedKey = KeyCode.Left;
                    else if (xDifference < -100)
                        _lastPressedKey = KeyCode.Right;
                }
                else // move over y axis
                {
                    if (yDifference > 100)
                        _lastPressedKey = KeyCode.Up;
                    else if (yDifference < -100)
                        _lastPressedKey = KeyCode.Down;
                }
            }
            _touchStart = null;
        }        
    }
}
