using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    /// <summary>
    /// Represents a position in the game
    /// </summary>
    public struct Point
    {
        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="x">The X position</param>
        /// <param name="y">The Y position</param>
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// The X position
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// The Y position
        /// </summary>
        public double Y { get; set; }
    }
}
