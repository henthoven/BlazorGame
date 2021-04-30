using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    /// <summary>
    /// Represents a size 
    /// </summary>
    public struct Size
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// The width
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// The height
        /// </summary>
        public double Height { get; set; }
    }
}
