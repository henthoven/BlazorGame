using GameEngine;
using System;
using System.Threading.Tasks;

namespace BlazorSnake.Game
{
    /// <summary>
    /// One part of the Snake GameObject
    /// </summary>
    public class SnakePart : GameObject<SnakeGameState>
    {
        private SnakeGame _snakeGame;
        private IGameObjectDrawer _gameObjectDrawer;

        /// <summary>
        /// The position in the level grid
        /// </summary>
        public Point GridPosition { get; private set; }

        /// <summary>
        /// The direction this snake part goes
        /// </summary>
        public SnakeDirection Direction { get; private set; }

        /// <summary>
        /// Flag that inidcates if this part is the head
        /// </summary>
        public bool IsHead { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="snakeGame">The SnakeGame that this GameObject is part of</param>
        /// <param name="gameObjectDrawer">The IGameObjectDrawer to use</param>
        /// <param name="xPos">The XPosition of this part</param>
        /// <param name="yPos">The YPosition of this part</param>
        public SnakePart(SnakeGame snakeGame, IGameObjectDrawer gameObjectDrawer, double xPos, double yPos)
        {
            _snakeGame = snakeGame;
            _gameObjectDrawer = gameObjectDrawer;

            AssetSourcePosition = new Point(0, 0);
            AssetSourceSize = new Size(128, 128);
            SetGridPositionAndDirection(new Point { X = xPos, Y = yPos }, SnakeDirection.Right);
        }

        /// <summary>
        /// Sets the position and direction of this SnakePart
        /// </summary>
        /// <param name="position">The position</param>
        /// <param name="direction">The direction</param>
        public void SetGridPositionAndDirection(Point position, SnakeDirection direction)
        {
            GridPosition = position;
            Position = new Point(position.X * _snakeGame.Level.BlockWidth, position.Y * _snakeGame.Level.BlockHeight);
            Size = _snakeGame.Level.BlockSize;
            Direction = direction;
        }

        /// <summary>
        /// Renders this gameobject
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        /// <returns></returns>
        public async override Task Render(float timeStamp)
        {
            await _gameObjectDrawer.DrawAsset(_snakeGame.Canvas, this);
        }

        /// <summary>
        /// Updates this GameObject
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        public override void Update(float timeStamp)
        {
            if (IsHead)
            {
                switch (Direction)
                {
                    case SnakeDirection.Left:
                        AssetSourcePosition = new Point(256, 256);
                        break;
                    case SnakeDirection.Up:
                        AssetSourcePosition = new Point(128, 256);
                        break;
                    case SnakeDirection.Right:
                        AssetSourcePosition = new Point(0, 256);
                        break;
                    case SnakeDirection.Down:
                        AssetSourcePosition = new Point(384, 256);
                        break;
                }
            }
        }

        /// <summary>
        /// Determines what asset should be drawn for this snakepart (for example a corner)
        /// 
        /// TODO: This method should be implemented simpler!
        /// </summary>
        /// <param name="nextPartDirection">The direction of the next part of the snake</param>
        public void DetermineCorner(SnakeDirection nextPartDirection)
        {
            if (Direction == nextPartDirection)
            {
                switch (Direction)
                {
                    case SnakeDirection.Left:
                        AssetSourcePosition = new Point(256, 0);
                        break;
                    case SnakeDirection.Up:
                        AssetSourcePosition = new Point(128, 0);
                        break;
                    case SnakeDirection.Right:
                        AssetSourcePosition = new Point(0, 0);
                        break;
                    case SnakeDirection.Down:
                        AssetSourcePosition = new Point(384, 0);
                        break;
                }
            }
            if (Direction == SnakeDirection.Right && nextPartDirection == SnakeDirection.Up)
            {
                AssetSourcePosition = new Point(128, 128);
            }
            else if (Direction == SnakeDirection.Right && nextPartDirection == SnakeDirection.Down)
            {
                AssetSourcePosition = new Point(256, 128);
            }
            else if (Direction == SnakeDirection.Left && nextPartDirection == SnakeDirection.Up)
            {
                AssetSourcePosition = new Point(0, 128);
            }
            else if (Direction == SnakeDirection.Left && nextPartDirection == SnakeDirection.Down)
            {
                AssetSourcePosition = new Point(384, 128);
            }
            else if (Direction == SnakeDirection.Up && nextPartDirection == SnakeDirection.Left)
            {
                AssetSourcePosition = new Point(256, 128);
            }
            else if (Direction == SnakeDirection.Up && nextPartDirection == SnakeDirection.Right)
            {
                AssetSourcePosition = new Point(384, 128);
            }
            else if (Direction == SnakeDirection.Down && nextPartDirection == SnakeDirection.Left)
            {
                AssetSourcePosition = new Point(128, 128);
            }
            else if (Direction == SnakeDirection.Down && nextPartDirection == SnakeDirection.Right)
            {
                AssetSourcePosition = new Point(0, 128);
            }
        }
    }
}
