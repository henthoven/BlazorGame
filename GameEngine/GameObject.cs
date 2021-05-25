using System.Threading.Tasks;

namespace GameEngine
{
    /// <summary>
    /// Base class for a GameObject
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public abstract class GameObject<TState> : IGameObject
    {
        /// <summary>
        /// The X and Y position of the asset in the source image
        /// </summary>
        public Point AssetSourcePosition { get; protected set; }
        /// <summary>
        /// The position to render this object on the target canvas
        /// </summary>
        public Point Position { get; set; }
        /// <summary>
        /// The size of the asset in the source image
        /// </summary>
        public Size AssetSourceSize { get; protected set; }
        /// <summary>
        /// The size to render the image at the target canvas
        /// </summary>
        public Size Size { get; set; }
    
        /// <summary>
        /// Updates this GameObject
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        public virtual void Update(float timeStamp)
        {
        }

        /// <summary>
        /// Renders this GameObject
        /// </summary>
        /// <param name="timeStamp">The current timestamp</param>
        /// <returns></returns>
        public virtual Task Render(float timeStamp)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Is called when this object is changed in state
        /// </summary>
        /// <param name="previousState">The previous state</param>
        /// <param name="newState">The new state</param>
        public virtual void OnEnterState(TState previousState, TState newState)
        {
        }
    }
}
