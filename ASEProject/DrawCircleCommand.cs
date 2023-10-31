using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEProject
{
    /// <summary>
    /// Represents a command to draw a circle in a drawing application.
    /// </summary>
    public class DrawCircleCommand : ICommand
    {
        /// <summary>
        /// Executes the command to draw a circle.
        /// </summary>
        /// <param name="commandList">The command list to which this command belongs.</param>
        /// <param name="parameters">An array of parameters that provide information about the circle (e.g., center coordinates and radius).</param>
        public void Execute(CommandList commandList, string[] parameters)
        {
            if (parameters.Length >= 2 && int.TryParse(parameters[1], out int radius))
            {
                commandList.DrawCircle(parameters);
            }
        }
    }
}
