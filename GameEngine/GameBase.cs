using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    /// <summary>
    /// Base class for a game
    /// </summary>
    /// <typeparam name="TState">The numeration that defines the different states of the Game and GameObjects</typeparam>
    public abstract class GameBase<TState> 
    {
        private TState _currentState = default(TState);
        protected Canvas2DContext _outputCanvasContext;
        protected Canvas2DContext _outputCanvasForCacheContext;

        /// <summary>
        /// The GameStates that the game uses 
        /// </summary>
        public Dictionary<TState, GameState<TState>> GameStates { get; private set; } = new Dictionary<TState, GameState<TState>>();

        /// <summary>
        /// The size of the game
        /// </summary>
        public Size Size { get; set; }
        
        /// <summary>
        /// The game height
        /// </summary>
        public double GameHeight => Size.Height-ScoreBoardHeight;

        /// <summary>
        /// The score board height
        /// 
        /// TODO: Should be moved the game specific implementation
        /// </summary>
        public int ScoreBoardHeight => 100;

        /// <summary>
        /// The Canvas to draw on
        /// </summary>
        public Canvas2DContext Canvas => _outputCanvasContext;

        /// <summary>
        /// The canvas to draw scenes on that should be cached and reused
        /// </summary>
        public Canvas2DContext CanvasForCache => _outputCanvasForCacheContext;

        /// <summary>
        /// The input manager
        /// </summary>
        public IInputManager InputManager { get; private set; }

        /// <summary>
        /// The GameTime
        /// </summary>
        public IGameTime GameTime { get; private set; }

        /// <summary>
        /// Flag that indicates if the game is resized
        /// </summary>
        public bool IsResized { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider">The serviceprovider to use</param>
        /// <param name="outputCanvasContext">The canvas to draw on</param>
        /// <param name="outputCanvasForCacheContext">The canvas to draw scenes on that should be cached</param>
        /// <param name="width">Width of the game</param>
        /// <param name="height">Height of the game</param>
        public GameBase(IServiceProvider serviceProvider, Canvas2DContext outputCanvasContext, Canvas2DContext outputCanvasForCacheContext, int width, int height)//, int blockWidth, int blockHeight)
        {
            Size = new Size(width, height);

            _outputCanvasContext = outputCanvasContext;
            _outputCanvasForCacheContext = outputCanvasForCacheContext;

            InputManager = (IInputManager)serviceProvider.GetService(typeof(IInputManager));
            GameTime = (IGameTime)serviceProvider.GetService(typeof(IGameTime));
        }

        /// <summary>
        /// Updates the game loop and calls the update and render method
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <param name="isWindowsResized">Flag if the game is resized</param>
        /// <returns>A completed task</returns>
        public async Task UpdateLoop(float timeStamp, int width, int height, bool isWindowsResized = false)
        {
            IsResized = isWindowsResized; 
            Size = new Size(width, height);

            GameTime.SetTimeStamp(timeStamp);
            Update(timeStamp);
            await Render(timeStamp);
        }

        /// <summary>
        /// Adds a game state for this game
        /// </summary>
        /// <param name="state">The state</param>
        /// <param name="gameState">The state containing all the game objects</param>
        protected void AddGameState(TState state, GameState<TState> gameState)
        {            
            GameStates[state] = gameState;
        }

        /// <summary>
        /// Updates the game
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        protected virtual void Update(float timeStamp)
        {
            foreach (var gameObject in GameStates[_currentState].GameObjects)
            {
                gameObject.Update(timeStamp);
            }
        }

        /// <summary>
        /// Renders the game
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        /// <returns>A completed task</returns>
        protected async virtual Task Render(float timeStamp)
        {
            await _outputCanvasContext.BeginBatchAsync();
            foreach (var gameObject in GameStates[_currentState].GameObjects)
            {
                await gameObject.Render(timeStamp);
            }
            await _outputCanvasContext.EndBatchAsync();
        }

        /// <summary>
        /// Changes the state of the game and notifies all gameobjects that the state is changed
        /// </summary>
        /// <param name="newState">The new state to move to</param>
        public void ChangeState(TState newState)
        {
            TState previousState = _currentState;
            _currentState = newState;
            foreach(var gameObject in GameStates[_currentState].GameObjects)
            {
                gameObject.OnEnterState(previousState, newState);
            }
        }
    }
}
