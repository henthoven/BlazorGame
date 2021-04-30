using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorSnake.Game
{
    /// <summary>
    /// The game over game object
    /// </summary>
    public class GameOver : GameObject<SnakeGameState>
    {
        private SnakeGame _snakeGame;
        private IInputManager _inputManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider">The serviceprovider to use to get dependencies</param>
        /// <param name="snakeGame">The snakegame where this GameObject is part of</param>
        public GameOver(IServiceProvider serviceProvider, SnakeGame snakeGame)
        {
            _snakeGame = snakeGame;
            _inputManager = (IInputManager)serviceProvider.GetService(typeof(IInputManager));
        }

        /// <summary>
        /// The render implementation for this GameObject.
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
            await _snakeGame.Canvas.FillTextAsync($"Game over!", _snakeGame.Size.Width / 2 - 50 , _snakeGame.Size.Height / 2 - 80);
            await _snakeGame.Canvas.FillTextAsync($"Total score: {_snakeGame.Level.Score}", _snakeGame.Size.Width / 2 - 100, _snakeGame.Size.Height / 2 - 40);
            await _snakeGame.Canvas.FillTextAsync($"Press enter to continue...", _snakeGame.Size.Width / 2 - 150, _snakeGame.Size.Height / 2);

            await base.Render(timeStamp);
        }

        /// <summary>
        /// The update method for this GameObject.
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        public override void Update(float timeStamp)
        {
            switch (_inputManager.LastPressedKey)
            {
                case KeyCode.Enter:
                    _snakeGame.ChangeState(SnakeGameState.MainMenu);
                    break;
            }

            base.Update(timeStamp);
        }

        /// <summary>
        /// Will be called to initialize the given state for this GameObject
        /// </summary>
        /// <param name="previousState">The previous state</param>
        /// <param name="newState">The new state</param>
        public override void OnEnterState(SnakeGameState previousState, SnakeGameState newState)
        {
            if (newState == SnakeGameState.GameOver)
            {
                _inputManager.ResetLastPressedKey();
            }

            base.OnEnterState(previousState, newState);
        }
    }
}
