using GameEngine;
using GameEngine.Sound;
using Howler.Blazor.Components;
using Howler.Blazor.Components.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorSnake.Game
{
    /// <summary>
    /// The snake level GameObject
    /// </summary>
    public class SnakeLevel : GameObject<SnakeGameState>
    {
        private SnakeGame _snakeGame;
        private LevelData _levelData;
        private List<LevelBlock> _levelBlocks = new List<LevelBlock>();
        private IGameObjectDrawer _canvasDrawer;
        private IGameTimer _gameTimer;
        private ISoundPlayer _soundPlayer;

        private int _applesEaten;
        private int _levelGridWidth;
        private int _levelGridHeight;
        private int _levelNumber;
        private int _score = 0;
        private bool _started;
        private bool _firstRender = true;
        private int _levelSoundId;

        /// <summary>
        /// Returns the size of a block in the level grid
        /// </summary>
        public Size BlockSize => new Size(BlockWidth, BlockHeight);
        /// <summary>
        /// Returns the width of a block in the level
        /// </summary>
        public double BlockWidth => _snakeGame.Size.Width / (double)_levelGridWidth;
        /// <summary>
        /// Returns the height of a block in the level
        /// </summary>
        public double BlockHeight => _snakeGame.Size.Width / (double)_levelGridWidth; 
        /// <summary>
        /// Returns the name of the loaded level
        /// </summary>
        public string Name => _levelData?.Name;
        /// <summary>
        /// Returns the number of apples that needs to be eaten to complete the level
        /// </summary>
        public int ApplesLeft => (int)_levelData?.AppleCount - _applesEaten;
        /// <summary>
        /// Returns the current score of the player
        /// </summary>
        public int Score => _score;
        /// <summary>
        /// Returns the number of seconds left for a possible time bonus
        /// </summary>
        public int TimeBonusUntil => (int)_levelData?.TimeBonusUntil;
        /// <summary>
        /// Returns the startposition for the snake in the loaded level
        /// </summary>
        public Point? StartPosition => _levelData?.SnakeStartPosition;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider">The serviceprovider to use</param>
        /// <param name="snakeGame">The SnakeGame that this level is part of</param>
        public SnakeLevel(IServiceProvider serviceProvider, SnakeGame snakeGame)
        {
            _snakeGame = snakeGame;
            _gameTimer = serviceProvider.GetRequiredService<IGameTimer>();
            _canvasDrawer = serviceProvider.GetRequiredService<IGameObjectDrawer>();
            _soundPlayer = serviceProvider.GetRequiredService<ISoundPlayer>();

            // background asset
            AssetSourcePosition = new Point(128, 384);
            AssetSourceSize = new Size(128, 128);
        }

        /// <summary>
        /// Initialized the level
        /// </summary>
        public void Initialize()
        {
            _levelNumber++;
            
            // temp
            if (_levelNumber > 4)
                _levelNumber = 1;
            //_levelNumber = 3;
            _levelBlocks = new List<LevelBlock>();
            _levelData = LoadLevelData(_levelNumber);
            _levelGridWidth = _levelData.Data.First().Count();
            _levelGridHeight = _levelData.Data.Count();
            _applesEaten = 0;
            _firstRender = true;

            _gameTimer.Reset();           

            int yPosition = 0;
            foreach (var line in _levelData.Data)
            {
                int xPosition = 0;
                foreach (var block in line)
                {
                    Point position = new Point { X = xPosition, Y = yPosition };
                    if (block == 1)
                    {
                        _levelBlocks.Add(new LevelBlock(_snakeGame, _snakeGame.CanvasForCache, _canvasDrawer, position, LevelBlockType.Wall));
                    }
                    else if (block == 2)
                    {
                        _levelBlocks.Add(new LevelBlock(_snakeGame, _snakeGame.Canvas, _canvasDrawer, position, LevelBlockType.Apple));
                    }
                    xPosition++;
                }
                yPosition++;
            }

            AddNewApple();
        }

        /// <summary>
        /// Getst the levelblock at the given position
        /// </summary>
        /// <param name="position">The position to get the levelblock from</param>
        /// <returns>The levelblock at the requested position</returns>
        public LevelBlock GetLevelBlock(Point position)
        {
            return _levelBlocks.FirstOrDefault(lb => lb.GridPosition.X == position.X && lb.GridPosition.Y == position.Y);
        }

        /// <summary>
        /// Adds the given points to the score
        /// </summary>
        /// <param name="score">The score to add</param>
        public void AddScore(int score)
        {
            _score += score;
        }

        /// <summary>
        /// Starts the level
        /// </summary>
        public void Start()
        {
            _started = true;
            Task.Run(async () => _levelSoundId = await _soundPlayer.Play("/sounds/levelbackgroundmusic.mp3", true));
        }

        /// <summary>
        /// Eats the apple at the given levelblock
        /// </summary>
        /// <param name="nextLevelBlock"></param>
        public void EatApple(LevelBlock nextLevelBlock)
        {
            _levelBlocks.Remove(nextLevelBlock);
            _applesEaten++;
            AddScore(25);

            Task.Run(async () => await _soundPlayer.Play("/sounds/eatapple.mp3"));

            AddNewApple();
        }

        /// <summary>
        /// Resets the game
        /// </summary>
        public void ResetGame()
        {
            _score = 0;
            _levelNumber = 0;
            _started = false;
        }

        /// <summary>
        /// Renders this GameObject
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        /// <returns>A completed task</returns>
        public async override Task Render(float timeStamp)
        {            
            await _snakeGame.Canvas.SetFillStyleAsync("white");
            await _snakeGame.Canvas.SetFontAsync("20px Comic Sans MS");

            // render blocks in level on the cache canvas. This only happens the first time or when
            // the game is resized. After that the cached canvas will be drawn on the real canvas.
            if (_firstRender || _snakeGame.IsResized)
            {
                Size = _snakeGame.Size;

                await _snakeGame.CanvasForCache.BeginBatchAsync();
                await _snakeGame.CanvasForCache.ClearRectAsync(0, 0, _snakeGame.Size.Width, _snakeGame.Size.Height);
                await _canvasDrawer.DrawAsset(_snakeGame.CanvasForCache, this);
                foreach (var levelBlock in _levelBlocks)
                {
                    levelBlock.Size = _snakeGame.Level.BlockSize;
                    levelBlock.Position = new Point(levelBlock.GridPosition.X * _snakeGame.Level.BlockWidth, levelBlock.GridPosition.Y * _snakeGame.Level.BlockHeight);
                    await levelBlock.Render(timeStamp);
                }
                await _snakeGame.CanvasForCache.EndBatchAsync();
                _firstRender = false;
            }

            // draw the cached canvas
            await _snakeGame.Canvas.BeginPathAsync();
            await _snakeGame.Canvas.DrawImageAsync(_snakeGame.CanvasForCache.Canvas, 0, 0, _snakeGame.Size.Width, _snakeGame.Size.Height);            
            foreach (var levelBlock in _levelBlocks.Where(lb => lb.LevelBlockType == LevelBlockType.Apple))
            {
                await levelBlock.Render(timeStamp);
            }

            await _snakeGame.Canvas.FillTextAsync($"Level {_levelNumber}: {Name}", 40, _snakeGame.GameHeight + 40);
            await _snakeGame.Canvas.FillTextAsync($"Apples left: {ApplesLeft}", 40, _snakeGame.GameHeight + 80);
            await _snakeGame.Canvas.FillTextAsync($"Score: {_score}", _snakeGame.Size.Width - 120, _snakeGame.GameHeight + 40);
            await _snakeGame.Canvas.FillTextAsync($"Time: {_gameTimer.GetElapsedTimeInSeconds(timeStamp)}", _snakeGame.Size.Width - 120, _snakeGame.GameHeight + 80);

        }

        /// <summary>
        /// Updates this GameObject
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        public override void Update(float timeStamp)
        {
            if (_started && !_gameTimer.IsStarted)
                _gameTimer.Start(timeStamp);
            if (_started && ApplesLeft <= 0)
            {
                _snakeGame.ChangeState(SnakeGameState.LevelComplete);
                _gameTimer.Stop(timeStamp);
                _started = false;
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
            if (previousState == SnakeGameState.MainMenu)
            {
                ResetGame();
            }
            if (newState == SnakeGameState.LevelStart)
            {
                Initialize();
            }
            else if (newState == SnakeGameState.LevelRunning)
            {
                Start();
            }            
            else if (newState == SnakeGameState.GameOver)
            {
                Task.Run(async () =>
                {
                    await _soundPlayer.Stop(_levelSoundId); 
                    await _soundPlayer.Play("/sounds/gameover.mp3");
                });
            }
            else if (newState == SnakeGameState.LevelComplete)
            {
                Task.Run(async () =>
                {
                    await _soundPlayer.Stop(_levelSoundId);
                    await _soundPlayer.Play("/sounds/levelcomplete.mp3");
                });

            }
            
            base.OnEnterState(previousState, newState);
        }

        #region private methods

        /// <summary>
        /// Adds a new apple to the level
        /// </summary>
        private void AddNewApple()
        {
            bool applePlaced = false;
            while (!applePlaced)
            {
                Point newApplePosition = new Point
                {
                    X = new Random().Next(1, _levelGridWidth - 1),
                    Y = new Random().Next(1, _levelGridHeight - 1)
                };

                if (!_levelBlocks.Any(lb => lb.GridPosition.X == newApplePosition.X && lb.GridPosition.Y == newApplePosition.Y)
                    && !_snakeGame.Snake.IsOnGridPosition(newApplePosition.X, newApplePosition.Y))
                {
                    _levelBlocks.Add(new LevelBlock(_snakeGame, _snakeGame.Canvas, _canvasDrawer, newApplePosition, LevelBlockType.Apple));
                    applePlaced = true;
                }
            }
        }

        /// <summary>
        /// Loads the leveldata for the given level numver
        /// </summary>
        /// <param name="levelNumber">The levelnumber to load</param>
        /// <returns>The raw leveldata</returns>
        private LevelData LoadLevelData(int levelNumber)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"BlazorSnake.Game.LevelData.level{levelNumber}.json";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return JsonConvert.DeserializeObject<LevelData>(reader.ReadToEnd());
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
