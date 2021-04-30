using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorSnake.Game
{
    /// <summary>
    /// The MainMenu GameObject
    /// </summary>
    public class MainMenu : GameObject<SnakeGameState>
    {
        private SnakeGame _snakeGame;
        private IInputManager _inputManager;
        private IGameObjectDrawer _gameObjectDrawer;
        private List<MenuItem> _menuItems = new List<MenuItem>();
        private int _selectedMenuItemIndex;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider">The service provider to use</param>
        /// <param name="snakeGame">The SnakeGame where this GameObject is part of</param>
        public MainMenu(IServiceProvider serviceProvider, SnakeGame snakeGame)
        {
            _snakeGame = snakeGame;
            _menuItems.AddRange(new List<MenuItem>
            {
                new MenuItem("Start new game", StartNewGame, true),
                new MenuItem("Credits", OpenCredits)
            });
            _inputManager = (IInputManager)serviceProvider.GetService(typeof(IInputManager));
            _gameObjectDrawer = (IGameObjectDrawer)serviceProvider.GetService(typeof(IGameObjectDrawer));

            AssetSourceSize = new Size(800, 600);
            Size = new Size(700, 800);
        }

        /// <summary>
        /// Renders this GameObject
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        /// <returns>A completed task</returns>
        public async override Task Render(float timeStamp)
        {
            await _snakeGame.Canvas.BeginPathAsync();
            await _snakeGame.Canvas.SetFillStyleAsync("Green");
            await _snakeGame.Canvas.SetFontAsync("60px Comic Sans MS");

            //await _snakeGame.Canvas.FillTextAsync($"BlazorSnake!", _snakeGame.Size.Width / 2 - 200, _snakeGame.GameHeight / 4);
            //await _snakeGame.Canvas.FillAsync();
            await _gameObjectDrawer.DrawAsset(_snakeGame.Canvas, "MainMenu", this);

            await _snakeGame.Canvas.BeginPathAsync();
            await _snakeGame.Canvas.SetFillStyleAsync("White");
            await _snakeGame.Canvas.SetFontAsync("32px Comic Sans MS");

            // draw menu items
            int menuItemDistance = 80;
            int index = 1;
            foreach (var menuItem in _menuItems)
            {                
                await _snakeGame.Canvas.SetFillStyleAsync(menuItem.IsSelected ? "Green": "White");
                await _snakeGame.Canvas.FillTextAsync(menuItem.Content, _snakeGame.Size.Width / 2 - 130, _snakeGame.GameHeight / 2 + menuItemDistance * ++index);
            }

            await _snakeGame.Canvas.FillAsync();
        }

        /// <summary>
        /// Updates this GameObject
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        public override void Update(float timeStamp)
        {
            base.Update(timeStamp);

            // Handle the input to navigate the menu
            switch (_inputManager.LastPressedKey)
            {
                case KeyCode.Up:
                    if (_selectedMenuItemIndex > 0)
                    {
                        _menuItems[_selectedMenuItemIndex].IsSelected = false;
                        _selectedMenuItemIndex--;
                        _menuItems[_selectedMenuItemIndex].IsSelected = true;
                    }
                    break;
                case KeyCode.Down:
                    if (_selectedMenuItemIndex < _menuItems.Count-1)
                    {
                        _menuItems[_selectedMenuItemIndex].IsSelected = false;
                        _selectedMenuItemIndex++;
                        _menuItems[_selectedMenuItemIndex].IsSelected = true;
                    }
                    break;
                case KeyCode.Enter:
                    _menuItems.First(i => i.IsSelected)?.Action();
                    break;
            }
            _inputManager.ResetLastPressedKey();
        }

        /// <summary>
        /// Is called when this gameobject changes state
        /// </summary>
        /// <param name="previousState">The previous state</param>
        /// <param name="newState">The new state</param>
        public override void OnEnterState(SnakeGameState previousState, SnakeGameState newState)
        {
            if (newState == SnakeGameState.MainMenu)
            {
                _inputManager.ResetLastPressedKey();
            }

            base.OnEnterState(previousState, newState);
        }

        /// <summary>
        /// Starts a new game
        /// </summary>
        private void StartNewGame()
        {
            _snakeGame.ChangeState(SnakeGameState.LevelStart);
        }

        /// <summary>
        /// Opens the credits screen
        /// </summary>
        private void OpenCredits()
        {

        }
    }
}
