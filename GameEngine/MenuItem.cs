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
        /// <param name="blink">Indicates if the text should blink</param>
        public MenuItem(string content, Action action, bool isSelected = false, bool blink = false)
        {
            Content = content;
            Action = action;
            IsSelected = isSelected;
            Blink = blink;
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

        /// <summary>
        /// Indicates if the text should blink
        /// </summary>
        public bool Blink { get; set; }
    }
}
