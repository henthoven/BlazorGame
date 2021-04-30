using Blazor.Extensions.Canvas.Canvas2D;
using GameEngine;
using System.Threading.Tasks;

namespace BlazorSnake.Game
{
    /// <summary>
    /// The GameObject that represents a single block in a level
    /// </summary>
    public class LevelBlock : GameObject<SnakeGameState>
    {
        private SnakeGame _snakeGame;
        private IGameObjectDrawer _canvasDrawer;
        private Canvas2DContext _canvasToUse;
   
        /// <summary>
        /// The position of this block in the level grid
        /// </summary>
        public Point GridPosition { get; private set; }
        
        /// <summary>
        /// The type of the level block
        /// </summary>
        public LevelBlockType LevelBlockType { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="snakeGame">The SnakeGame that this LevelBlock is part of</param>
        /// <param name="canvasToUse">The canvas to draw itself on</param>
        /// <param name="canvasDrawer">The canvas drawer to use</param>
        /// <param name="position">The absolute position where this LevelBlock is</param>
        /// <param name="levelBlockType">The type of the LevelBlock</param>
        public LevelBlock(SnakeGame snakeGame, Canvas2DContext canvasToUse, IGameObjectDrawer canvasDrawer, Point position, LevelBlockType levelBlockType)
        {
            _snakeGame = snakeGame;
            _canvasToUse = canvasToUse;
            _canvasDrawer = canvasDrawer;
            LevelBlockType = levelBlockType;

            GridPosition = position;
            Position = new Point(position.X * _snakeGame.Level.BlockWidth, position.Y * _snakeGame.Level.BlockHeight);
            Size = _snakeGame.Level.BlockSize;
            AssetSourceSize = new Size(128, 128);

            if (levelBlockType == LevelBlockType.Wall)
                AssetSourcePosition = new Point(0, 384);
            else if (levelBlockType == LevelBlockType.Apple)
                AssetSourcePosition = new Point(256, 384);
        }

        /// <summary>
        /// Renders this GameObject
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        /// <returns>A completed task</returns>
        public async override Task Render(float timeStamp)
        {
            await _canvasToUse.BeginPathAsync();
            await _canvasDrawer.DrawAsset(_canvasToUse, this);
        }
    }
}
