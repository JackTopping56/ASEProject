﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEProject
{
    /// <summary>
    /// Represents a command to draw a rectangle in a drawing application.
    /// </summary>
    public class DrawRectangleCommand : ICommand
    {
        /// <summary>
        /// Executes the command to draw a rectangle.
        /// </summary>
        /// <param name="commandList">The command list to which this command belongs.</param>
        /// <param name="parameters">An array of parameters that provide information about the rectangle (e.g., top-left coordinates, width, and height).</param>
        public void Execute(CommandList commandList, string[] parameters)
        {
            if (parameters.Length >= 3 && int.TryParse(parameters[1], out int width) && int.TryParse(parameters[2], out int height))
            {
                commandList.DrawRectangle(parameters);
            }
        }
    }
}
