using GameEngine;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorSnake.Game
{
    /// <summary>
    /// The Snake GameObject
    /// </summary>
    public class Snake : GameObject<SnakeGameState>
    {
        private SnakeGame _snakeGame;
        private IInputManager _inputManager;
        private IGameObjectDrawer _gameObjectDrawer;
        private SnakeDirection _direction = SnakeDirection.Right;
        private Point _startPosition;
        private List<SnakePart> _snakeParts;
        private int _speed = 6;
        private int _growAmount = 0;
        private bool _started;
        private float _lastUpdateTime = 0;
        private KeyCode _lastPressedKey;

        /// <summary>
        /// The head (first part) of the snake
        /// </summary>
        public SnakePart Head => _snakeParts.First();

        /// <summary>
        /// Flag that indicates if the snake is alive
        /// </summary>
        public bool IsAlive => _speed > 0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider">The serviceprovider to use</param>
        /// <param name="snakeGame">The SnakeGame where this Snake is part of</param>
        public Snake(IServiceProvider serviceProvider, SnakeGame snakeGame)
        {
            _snakeGame = snakeGame;
            _inputManager = serviceProvider.GetRequiredService<IInputManager>();
            _gameObjectDrawer = serviceProvider.GetRequiredService<IGameObjectDrawer>();
            _startPosition = _snakeGame.Level.StartPosition ?? new Point { X = 10, Y = 10 };

            Initialize();
        }

        /// <summary>
        /// Sets the direction of the snake based on the given keycode
        /// </summary>
        /// <param name="keyCode">The keycode of the key that was pressed</param>
        public void SetDirection(KeyCode keyCode)
        {
            if ((keyCode == KeyCode.Left && _direction != SnakeDirection.Right)
                || (keyCode == KeyCode.Right && _direction != SnakeDirection.Left)
                || (keyCode == KeyCode.Up && _direction != SnakeDirection.Down)
                || (keyCode == KeyCode.Down && _direction != SnakeDirection.Up))
            {
                _direction = (SnakeDirection)Enum.Parse(typeof(SnakeDirection), keyCode.ToString()); // TODO!!!
            }
        }

        /// <summary>
        /// Starts the snake
        /// </summary>
        public void Start()
        {
            _started = true;
            _speed = 6;
        }

        /// <summary>
        /// Initializes the snake
        /// </summary>
        internal void Initialize()
        {
            _startPosition = _snakeGame.Level.StartPosition ?? new Point { X = 10, Y = 10 };
            _direction = SnakeDirection.Right;
            _snakeParts = new List<SnakePart>()
                {
                    new SnakePart(_snakeGame, _gameObjectDrawer, _startPosition.X, _startPosition.Y) { IsHead = true },
                    new SnakePart(_snakeGame, _gameObjectDrawer, _startPosition.X-1, _startPosition.Y),
                    new SnakePart(_snakeGame, _gameObjectDrawer, _startPosition.X-2, _startPosition.Y),
                };
            _snakeParts[0].Update(0);// TODO hack
            _started = false;
        }

        /// <summary>
        /// Renders the snake
        /// </summary>
        /// <param name="timeStamp">The current timestam</param>
        /// <returns>A completed task</returns>
        public async override Task Render(float timeStamp)
        {
            foreach (SnakePart snakePart in _snakeParts)
            {
                await snakePart.Render(timeStamp);
            }
        }

        /// <summary>
        /// Updates the snake
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        public override void Update(float timeStamp)
        {
            _lastPressedKey = _inputManager.LastPressedKey;            
            // only update the snake when it is alive and a certain time has passed (to manage the speed of the snake)
            if (_started && IsAlive && (timeStamp - _lastUpdateTime) * _speed > 750f)
            {
                _lastUpdateTime = timeStamp;

                SnakePart newFirstPart = _snakeParts.Last();
                SnakePart firstPart = _snakeParts.First();
                if (_growAmount > 0)
                {
                    newFirstPart = new SnakePart(_snakeGame, _gameObjectDrawer, 0, 0);
                    _growAmount--;
                }
                else
                {
                    _snakeParts.Remove(newFirstPart);
                }
                SetSpeed();
                SetDirection(_lastPressedKey);
                switch (_direction)
                {
                    case SnakeDirection.Left:
                        newFirstPart.SetGridPositionAndDirection(new Point { X = firstPart.GridPosition.X - 1, Y = firstPart.GridPosition.Y }, _direction);
                        break;
                    case SnakeDirection.Up:
                        newFirstPart.SetGridPositionAndDirection(new Point { X = firstPart.GridPosition.X, Y = firstPart.GridPosition.Y - 1 }, _direction);
                        break;
                    case SnakeDirection.Right:
                        newFirstPart.SetGridPositionAndDirection(new Point { X = firstPart.GridPosition.X + 1, Y = firstPart.GridPosition.Y }, _direction);
                        break;
                    case SnakeDirection.Down:
                        newFirstPart.SetGridPositionAndDirection(new Point { X = firstPart.GridPosition.X, Y = firstPart.GridPosition.Y + 1 }, _direction);
                        break;
                }
                newFirstPart.IsHead = true;
                firstPart.IsHead = false;
                firstPart.DetermineCorner(newFirstPart.Direction);                
                _snakeParts.Insert(0, newFirstPart);
                newFirstPart.Update(timeStamp);
                
                HandleCollisions();
            }
        }

        /// <summary>
        /// Initialized the snake for the given new state
        /// </summary>
        /// <param name="previousState">The previous state</param>
        /// <param name="newState">The new state</param>
        public override void OnEnterState(SnakeGameState previousState, SnakeGameState newState)
        {
            if (newState == SnakeGameState.LevelRunning)
            {
                Start();
            }
            else if (newState == SnakeGameState.LevelStart)
            {
                Initialize();
            }
            else if (newState == SnakeGameState.LevelComplete)
            {
                Stop();
            }
            else if (newState == SnakeGameState.GameOver)
            {
                Stop();
            }

            base.OnEnterState(previousState, newState);
        }

        /// <summary>
        /// Stops the snake
        /// </summary>
        public void Stop()
        {
            _started = false;
        }

        #region private methods

        /// <summary>
        /// Makes the snake die
        /// </summary>
        private void Die()
        {
            _speed = 0;
            _snakeGame.ChangeState(SnakeGameState.GameOver);
        }

        /// <summary>
        /// Makes the snake grow with the given amount
        /// </summary>
        /// <param name="amount"></param>
        private void Grow(int amount)
        {
            _growAmount += amount;
        }

        /// <summary>
        /// Sets the speed of the snake based on the speed keys that are pressed
        /// </summary>
        private void SetSpeed()
        {
            // TODO: get min and max from somewhere else
            int maxSpeed = 15;
            int minSpeed = 3;
            if (_inputManager.IsPressed(KeyCode.A) && _speed < maxSpeed)
                _speed += 1;
            if (_inputManager.IsPressed(KeyCode.Z) && _speed > minSpeed)
                _speed -= 1;
        }

        /// <summary>
        /// Handles collisions (with all kind of other objects including itself)
        /// </summary>
        private void HandleCollisions()
        {
            LevelBlock nextLevelBlock = _snakeGame.Level.GetLevelBlock(Head.GridPosition);
            if (nextLevelBlock?.LevelBlockType == LevelBlockType.Wall // collision with level
                || _snakeParts.Any(sp => sp != Head && sp.GridPosition.X == Head.GridPosition.X && sp.GridPosition.Y == Head.GridPosition.Y))
            {
                Die();
            }
            else if (nextLevelBlock?.LevelBlockType == LevelBlockType.Apple)
            {
                Grow(3);
                _snakeGame.Level.EatApple(nextLevelBlock);
            }
        }

        /// <summary>
        /// Returns if any part of the snake is on the given position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsOnGridPosition(double x, double y)
        {
            return _snakeParts.Any(sp => sp.GridPosition.X == x && sp.GridPosition.Y == y);
        }
        #endregion
    }
}
