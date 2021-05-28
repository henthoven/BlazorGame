using GameEngine;
using GameEngine.Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorSnake.Game
{
    /// <summary>
    /// The SnakeLevelStart GameObject
    /// </summary>
    public class SnakeLevelStart : GameObject<SnakeGameState>
    {
        private const int SECONDS_BEFORE_START = 3;

        private SnakeGame _snakeGame;
        private IGameTimer _gameTimer;
        private IInputManager _inputManager;
        private ISoundPlayer _soundPlayer;
        private DateTime _startTime;
        private int _secondsLeft;
        private int _elapsedSeconds;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider">The serviceprovider to use</param>
        /// <param name="snakeGame">The snake game that this GameObject is part of</param>
        public SnakeLevelStart(IServiceProvider serviceProvider, SnakeGame snakeGame)
        {
            _snakeGame = snakeGame;
            _gameTimer = (IGameTimer)serviceProvider.GetService(typeof(IGameTimer));
            _inputManager = (IInputManager)serviceProvider.GetService(typeof(IInputManager));
            _soundPlayer = (ISoundPlayer)serviceProvider.GetService(typeof(ISoundPlayer));
        }

        /// <summary>
        /// Renders this GameObject
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        /// <returns>A completed task</returns>
        public async override Task Render(float timeStamp)
        {
            await _snakeGame.Canvas.BeginPathAsync();
            await _snakeGame.Canvas.RectAsync(_snakeGame.Size.Width / 2 - 200, _snakeGame.GameHeight / 2 - 50, 400, 100);
            await _snakeGame.Canvas.SetFillStyleAsync("brown"); // TODO
            await _snakeGame.Canvas.FillAsync();

            await _snakeGame.Canvas.BeginPathAsync();
            await _snakeGame.Canvas.SetFillStyleAsync("white"); // TODO
            await _snakeGame.Canvas.FillTextAsync($"Get ready!", _snakeGame.Size.Width / 2 - 50 , _snakeGame.Size.Height / 2 - 80);

            await _snakeGame.Canvas.BeginPathAsync();
            await _snakeGame.Canvas.SetFontAsync($"{50 + _elapsedSeconds*12}px Comic Sans MS");
            if (_secondsLeft <= 0)
            {
                await _snakeGame.Canvas.SetFillStyleAsync("red"); // TODO
                await _snakeGame.Canvas.FillTextAsync($"GO!", _snakeGame.Size.Width / 2 - 70, _snakeGame.Size.Height / 2 - 10);
            }
            else
            {
                await _snakeGame.Canvas.FillTextAsync($"{_secondsLeft}", _snakeGame.Size.Width / 2 - 10, _snakeGame.Size.Height / 2 - 10);
            }

            await base.Render(timeStamp);
        }

        /// <summary>
        /// Updates this GameObject
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        public override void Update(float timeStamp)
        {
            _elapsedSeconds = (DateTime.Now - _startTime).Seconds;
            _secondsLeft = SECONDS_BEFORE_START - _elapsedSeconds;
            if (_secondsLeft < 0)
            {
                _snakeGame.ChangeState(SnakeGameState.LevelRunning);
            }

            base.Update(timeStamp);
        }

        /// <summary>
        /// Initialized this GameObject based on the state that is entered
        /// </summary>
        /// <param name="previousState">The previous state</param>
        /// <param name="newState">The new state</param>
        public override void OnEnterState(SnakeGameState previousState, SnakeGameState newState)
        {
            if (newState == SnakeGameState.LevelStart)
            {
                Initialize();
            }
            base.OnEnterState(previousState, newState);
        }

        /// <summary>
        /// Initialized this GameObject
        /// </summary>
        private void Initialize()
        {
            _startTime = DateTime.Now;
            _secondsLeft = SECONDS_BEFORE_START;
            _gameTimer.Reset();
            _inputManager.ResetLastPressedKey();

            _soundPlayer.Play("/sounds/321go.mp3");
        }
    }
}
