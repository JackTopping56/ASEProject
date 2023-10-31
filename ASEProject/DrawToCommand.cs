using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEProject
{
    /// <summary>
    /// Represents a command to draw to a specific point in a drawing application.
    /// </summary>
    public class DrawToCommand : ICommand
    {
        /// <summary>
        /// Executes the command to draw to a specific point.
        /// </summary>
        /// <param name="commandList">The command list to which this command belongs.</param>
        /// <param name="parameters">An array of parameters that provide the destination coordinates (X and Y).</param>
        public void Execute(CommandList commandList, string[] parameters)
        {
            if (parameters.Length >= 3 && int.TryParse(parameters[1], out int x) && int.TryParse(parameters[2], out int y))
            {
                commandList.DrawTo(parameters);
            }
        }
    }
}
