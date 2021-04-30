using Blazor.Extensions.Canvas.Canvas2D;
using GameEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorSnake.Game
{
    /// <summary>
    /// The SnakeGame class contains the main logic for the game
    /// </summary>
    public class SnakeGame : GameBase<SnakeGameState>
    {
        /// <summary>
        /// The snake that moves trought the levels
        /// </summary>
        public Snake Snake { get; private set; }
        /// <summary>
        /// The level that is shown while playing
        /// </summary>
        public SnakeLevel Level { get; private set; }
        /// <summary>
        /// The part that is shown when a level is started
        /// </summary>
        public SnakeLevelStart LevelStart { get; private set; }
        /// <summary>
        /// The part that is shown when a level is completed
        /// </summary>
        public SnakeLevelComplete LevelComplete { get; private set; }
        /// <summary>
        /// The gameover screen when a player dies
        /// </summary>
        public GameOver GameOver { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider">The serviceprovider to use</param>
        /// <param name="outputCanvasContext">The canvas to use to draw everything on</param>
        /// <param name="outputCanvasForCacheContext">A canvas to cache stuff on so it does not need to be redrawn everytime</param>
        /// <param name="width">The width of the game</param>
        /// <param name="height">The height of the game</param>
        public SnakeGame(IServiceProvider serviceProvider, Canvas2DContext outputCanvasContext, Canvas2DContext outputCanvasForCacheContext, int width, int height)
            : base(serviceProvider, outputCanvasContext, outputCanvasForCacheContext, width, height)
        {
            Level = new SnakeLevel(serviceProvider, this);
            LevelStart = new SnakeLevelStart(serviceProvider, this);
            LevelComplete = new SnakeLevelComplete(serviceProvider, this);
            GameOver = new GameOver(serviceProvider, this);
            Snake = new Snake(serviceProvider, this);
                        
            AddGameState(SnakeGameState.MainMenu, new GameState<SnakeGameState>(new List<GameObject<SnakeGameState>>
            {
                new MainMenu(serviceProvider, this),
            }));

            AddGameState(SnakeGameState.LevelRunning, new GameState<SnakeGameState>(new List<GameObject<SnakeGameState>>
            {
                Level,
                Snake
            }));

            AddGameState(SnakeGameState.LevelStart, new GameState<SnakeGameState>(new List<GameObject<SnakeGameState>>
            {
                Level,
                Snake,
                LevelStart
            }));

            AddGameState(SnakeGameState.LevelComplete, new GameState<SnakeGameState>(new List<GameObject<SnakeGameState>>
            {
                Level,
                Snake,
                LevelComplete
            }));

            AddGameState(SnakeGameState.GameOver, new GameState<SnakeGameState>(new List<GameObject<SnakeGameState>>
            {
                Level,
                Snake,
                GameOver
            }));

            ChangeState(SnakeGameState.MainMenu);
        }

        /// <summary>
        /// Updates the game
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        protected override void Update(float timeStamp)
        {
            base.Update(timeStamp);
        }

        /// <summary>
        /// Renders the game
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        /// <returns>A completed task</returns>
        protected async override Task Render(float timeStamp)
        {
            await _outputCanvasContext.BeginBatchAsync();
            await _outputCanvasContext.ClearRectAsync(0, 0, Size.Width, Size.Height);
            await _outputCanvasContext.SetFillStyleAsync("lightblue");
            await _outputCanvasContext.FillRectAsync(0, 0, Size.Width, Size.Height);

            await base.Render(timeStamp);

            await _outputCanvasContext.BeginBatchAsync();
            await _outputCanvasContext.SetFillStyleAsync("white");
            await _outputCanvasContext.SetFontAsync("20px Comic Sans MS");

            await _outputCanvasContext.FillTextAsync($"FPS: {Math.Round(GameTime.Fps)}", Size.Width - 100, 40);

            await _outputCanvasContext.EndBatchAsync();
        }
    }
}
