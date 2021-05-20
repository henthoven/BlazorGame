using GameEngine;
using GameEngine.Sound;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorSnake.Game
{
    /// <summary>
    /// The SnakeLevelComplete GameObject
    /// </summary>
    public class SnakeLevelComplete : GameObject<SnakeGameState>
    {
        private SnakeGame _snakeGame;
        private IInputManager _inputManager;
        private IGameTimer _gameTimer;
        private ISoundPlayer _soundPlayer;
        private bool _bonusReceived;
        private int _bonus;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider">The serviceprovider to use</param>
        /// <param name="snakeGame">The snakegame that this GameObject is part of</param>
        public SnakeLevelComplete(IServiceProvider serviceProvider, SnakeGame snakeGame)
        {
            _snakeGame = snakeGame;
            _inputManager = serviceProvider.GetRequiredService<IInputManager>();
            _gameTimer = serviceProvider.GetRequiredService<IGameTimer>();
            _soundPlayer = serviceProvider.GetRequiredService<ISoundPlayer>();
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
            await _snakeGame.Canvas.SetFillStyleAsync("black"); // TODO
            await _snakeGame.Canvas.FillAsync();

            await _snakeGame.Canvas.BeginPathAsync();
            await _snakeGame.Canvas.SetFillStyleAsync("white"); // TODO
            await _snakeGame.Canvas.FillTextAsync($"Well done!", _snakeGame.Size.Width / 2 - 50 , _snakeGame.Size.Height / 2 - 80);
            await _snakeGame.Canvas.FillTextAsync($"Time bonus: {_bonus}", _snakeGame.Size.Width / 2 - 100, _snakeGame.Size.Height / 2 - 40);
            await _snakeGame.Canvas.FillTextAsync($"Press enter to continue to the next level...", _snakeGame.Size.Width / 2 - 200, _snakeGame.Size.Height / 2);

            await base.Render(timeStamp);
        }

        /// <summary>
        /// Updates this GameObject
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        public override void Update(float timeStamp)
        {
            switch (_inputManager.LastPressedKey)
            {
                case KeyCode.Enter:
                    _snakeGame.Level.AddScore(_bonus);
                    _snakeGame.ChangeState(SnakeGameState.LevelStart);
                    break;
            }
            if (!_bonusReceived)
            {
                if (_gameTimer.GetElapsedTimeInSeconds(timeStamp) < _snakeGame.Level.TimeBonusUntil)
                    _bonus = (_snakeGame.Level.TimeBonusUntil - _gameTimer.GetElapsedTimeInSeconds(timeStamp)) * 10;
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
            if (newState == SnakeGameState.LevelComplete)
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
            _bonusReceived = false;
            _bonus = 0;
            _inputManager.ResetLastPressedKey();            
        }
    }
}
