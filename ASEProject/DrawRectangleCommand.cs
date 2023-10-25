using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEProject
{

    public class DrawRectangleCommand : ICommand
    {
        public void Execute(CommandList commandList, string[] parameters)
        {
            if (parameters.Length >= 3 && int.TryParse(parameters[1], out int width) && int.TryParse(parameters[2], out int height))
            {
                commandList.DrawRectangle(parameters);
            }
        }
    }
}
