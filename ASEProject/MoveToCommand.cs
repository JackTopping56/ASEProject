using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ASEProject
{
    /// <summary>
    /// Represents a command to move the drawing position to a specific point in a drawing application.
    /// </summary>
    public class MoveToCommand : ICommand
    {
        /// <summary>
        /// Executes the command to move the drawing position to a specific point.
        /// </summary>
        /// <param name="commandList">The command list to which this command belongs.</param>
        /// <param name="parameters">An array of parameters that provide the destination coordinates (X and Y).</param>
        public void Execute(CommandList commandList, string[] parameters)
        {
            if (parameters.Length >= 3 && int.TryParse(parameters[1], out int x) && int.TryParse(parameters[2], out int y))
            {
                commandList.MoveTo(parameters);
            }
        }
    }
}
