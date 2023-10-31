using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEProject
{
    /// <summary>
    /// Represents a command that changes the fill mode of a command list.
    /// </summary>
    public class ChangeFillModeCommand : ICommand
    {
        /// <summary>
        /// Executes the command to change the fill mode of a specified command list.
        /// </summary>
        /// <param name="commandList">The command list to modify.</param>
        /// <param name="parameters">An array of parameters specifying the new fill mode.</param>
        public void Execute(CommandList commandList, string[] parameters)
        {
            if (parameters.Length >= 2)
            {
                commandList.ChangeFillMode(parameters);
            }
        }
    }
}
