using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEProject
{
    /// <summary>
    /// Represents a command that clears the drawing in a drawing application.
    /// </summary>
    public class ClearCommand : ICommand
    {
        /// <summary>
        /// Executes the command to clear the drawing.
        /// </summary>
        /// <param name="commandList">The command list to which this command belongs.</param>
        /// <param name="parameters">An array of parameters (not used in this command).</param>
        public void Execute(CommandList commandList, string[] parameters)
        {
            commandList.Clear();
        }
    }
}
