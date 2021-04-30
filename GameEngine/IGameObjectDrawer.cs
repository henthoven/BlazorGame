using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using System.Drawing;
using System.Threading.Tasks;

namespace GameEngine
{
    /// <summary>
    /// Interface for a GameObjectDrawer
    /// </summary>
    public interface IGameObjectDrawer
    {
        /// <summary>
        /// Initializes this GameObjectDrawer with the given ELementReference
        /// </summary>
        /// <param name="assets">The assets</param>
        void Initialize(ElementReference assets);

        /// <summary>
        /// Adds extra assets to the GameObjectDrawer under a given key
        /// </summary>
        /// <param name="assetKey">The key for the asset</param>
        /// <param name="assets">The assets</param>
        void AddExtraAsset(string assetKey, ElementReference assets);

        /// <summary>
        /// Draws the given GameObject onto the given canvas
        /// </summary>
        /// <param name="canvasToDrawOn">The canvas to draw on</param>
        /// <param name="gameObject">The GameObject to draw</param>
        /// <returns>A completed task</returns>
        Task DrawAsset(Canvas2DContext canvasToDrawOn, IGameObject gameObject);

        /// <summary>
        /// Draws an asset from a given position in the default assets to the given position on the canvas
        /// </summary>
        /// <param name="canvasToDrawOn">The canvas to draw on</param>
        /// <param name="gameObject">The GameObject to draw</param>
        /// <param name="desitnationPosition">The desitination to draw the asset</param>
        /// <param name="destinationSize">The size to draw the asset</param>
        /// <param name="sourcePosition">The position in the source image</param>
        /// <param name="sourceSize">The size in the source image</param>
        /// <returns>A completed task</returns>
        Task DrawAsset(Canvas2DContext canvasToDrawOn, Point sourcePosition, Size sourceSize, Point desitnationPosition, Size destinationSize);

        /// <summary>
        /// Draws the given GameObject onto the given canvas using the assetKey as asset source
        /// </summary>
        /// <param name="canvasToDrawOn">The canvas to draw on</param>
        /// <param name="assetKey">The key of the asset source to use</param>
        /// <param name="gameObject">The GameObject to draw</param>
        /// <returns>A completed task</returns>
        Task DrawAsset(Canvas2DContext canvasToDrawOn, string assetKey, IGameObject gameObject);

        /// <summary>
        /// Draws an asset from a given position in the given assets to the given position on the canvas
        /// </summary>
        /// <param name="canvasToDrawOn">The canvas to draw on</param>
        /// <param name="desitnationPosition">The desitination to draw the asset</param>
        /// <param name="destinationSize">The size to draw the asset</param>
        /// <param name="sourcePosition">The position in the source image</param>
        /// <param name="sourceSize">The size in the source image</param>
        /// <param name="assets">The asset to get the image from</param>
        /// <returns>A completed task</returns>
        Task DrawAsset(Canvas2DContext canvasToDrawOn, Point sourcePosition, Size sourceSize, Point desitnationPosition, Size destinationSize, ElementReference assets);        
    }
}