using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    /// <summary>
    /// Represents a menu item 
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">The content to show</param>
        /// <param name="action">The action to executed when the item is chosen</param>
        /// <param name="isSelected">Indicates if the item is default selected</param>
        public MenuItem(string content, Action action, bool isSelected = false)
        {
            Content = content;
            Action = action;
            IsSelected = isSelected;
        }

        /// <summary>
        /// The content
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Flag that indicates if the item is selected
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// The action to execute when chosen
        /// </summary>
        public Action Action { get; private set; }
    }
}
