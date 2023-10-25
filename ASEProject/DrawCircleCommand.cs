using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEProject
{
    public class DrawCircleCommand : ICommand
    {
        public void Execute(CommandList commandList, string[] parameters)
        {
            if (parameters.Length >= 2 && int.TryParse(parameters[1], out int radius))
            {
                commandList.DrawCircle(parameters);
            }
        }
    }
}
