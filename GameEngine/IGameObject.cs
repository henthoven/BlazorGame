using System.Drawing;

namespace GameEngine
{
    /// <summary>
    /// Interface for a GameObject
    /// </summary>
    public interface IGameObject
    {
        /// <summary>
        /// The source position of the asset for this game object in the source image
        /// </summary>
        Point AssetSourcePosition { get; }
        /// <summary>
        /// The position to draw this GameObject to on the target canvas
        /// </summary>
        Point Position { get; }
        /// <summary>
        /// The size of the asset in the source file
        /// </summary>
        Size AssetSourceSize { get; }
        /// <summary>
        /// The size to draw the asset on the target canvas
        /// </summary>
        Size Size { get; }
    }
}