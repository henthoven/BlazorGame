using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameEngine
{
    /// <summary>
    /// Class that is able to draw GameObjects on a canvas
    /// </summary>
    public class GameObjectDrawer : IGameObjectDrawer
    {
        private ElementReference? _assets;
        private Dictionary<string, ElementReference> _extraAssets = new Dictionary<string, ElementReference>();

        /// <summary>
        /// Initializes this GameObjectDrawer with the given ELementReference
        /// </summary>
        /// <param name="assets">The assets</param>
        public void Initialize(ElementReference assets)
        {
            _assets = assets;
        }

        /// <summary>
        /// Adds extra assets to the GameObjectDrawer under a given key
        /// </summary>
        /// <param name="assetKey">The key for the asset</param>
        /// <param name="assets">The assets</param>
        public void AddExtraAsset(string assetKey, ElementReference assets)
        {
            _extraAssets[assetKey] = assets;
        }

        /// <summary>
        /// Draws the given GameObject onto the given canvas
        /// </summary>
        /// <param name="canvasToDrawOn">The canvas to draw on</param>
        /// <param name="gameObject">The GameObject to draw</param>
        /// <returns>A completed task</returns>
        public async Task DrawAsset(Canvas2DContext canvasToDrawOn, IGameObject gameObject)
        {
            ValidateInitialized();
            await DrawAsset(canvasToDrawOn, gameObject.AssetSourcePosition, gameObject.AssetSourceSize, gameObject.Position, gameObject.Size);
        }

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
        public async Task DrawAsset(Canvas2DContext canvasToDrawOn, Point sourcePosition, Size sourceSize, Point desitnationPosition, Size destinationSize)
        {
            ValidateInitialized();
            await canvasToDrawOn.DrawImageAsync(_assets.Value, sourcePosition.X, sourcePosition.Y, sourceSize.Width, sourceSize.Height, desitnationPosition.X, desitnationPosition.Y, destinationSize.Width, destinationSize.Height);
        }

        /// <summary>
        /// Draws the given GameObject onto the given canvas using the assetKey as asset source
        /// </summary>
        /// <param name="canvasToDrawOn">The canvas to draw on</param>
        /// <param name="assetKey">The key of the asset source to use</param>
        /// <param name="gameObject">The GameObject to draw</param>
        /// <returns>A completed task</returns>
        public async Task DrawAsset(Canvas2DContext canvasToDrawOn, string assetKey, IGameObject gameObject)
        {
            ValidateAssetKey(assetKey);
            await DrawAsset(canvasToDrawOn, gameObject.AssetSourcePosition, gameObject.AssetSourceSize, gameObject.Position, gameObject.Size, _extraAssets[assetKey]);
        }

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
        public async Task DrawAsset(Canvas2DContext canvasToDrawOn, Point sourcePosition, Size sourceSize, Point desitnationPosition, Size destinationSize, ElementReference assets)
        {
            await canvasToDrawOn.DrawImageAsync(assets, sourcePosition.X, sourcePosition.Y, sourceSize.Width, sourceSize.Height, desitnationPosition.X, desitnationPosition.Y, destinationSize.Width, destinationSize.Height);
        }

        /// <summary>
        /// Validates if assets are loaded
        /// </summary>
        private void ValidateInitialized()
        {
            if (_assets == null)
                throw new ApplicationException($"The CanvasDrawer is not initialized. Make sure you initialize it with a valid ElementRefernce containing the assets.");
        }

        /// <summary>
        /// Validates if an assetkey is availible
        /// </summary>
        /// <param name="assetKey">The key to validate</param>
        private void ValidateAssetKey(string assetKey)
        {
            if (!_extraAssets.ContainsKey(assetKey))
                throw new ApplicationException($"The asset with key '{assetKey}' was not found");
        }
    }
}
