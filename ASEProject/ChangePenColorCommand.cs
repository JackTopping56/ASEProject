using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEProject
{
    /// <summary>
    /// Represents a command that changes the pen color in a drawing application.
    /// </summary>
    public class ChangePenColorCommand : ICommand
    {
        /// <summary>
        /// Executes the command to change the pen color.
        /// </summary>
        /// <param name="commandList">The command list to which this command belongs.</param>
        /// <param name="parameters">An array of parameters that provide information about the new pen color.</param>
        public void Execute(CommandList commandList, string[] parameters)
        {
            if (parameters.Length >= 2)
            {
                commandList.ChangePenColor(parameters);
            }
        }
    }
}
