using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEProject
{
    public class DrawTriangleCommand : ICommand
    {
        public void Execute(CommandList commandList, string[] parameters)
        {
            if (parameters.Length >= 6 && int.TryParse(parameters[1], out int width)
                && int.TryParse(parameters[2], out int height)
                && int.TryParse(parameters[3], out int x2)
                && int.TryParse(parameters[4], out int y2))
            {
                commandList.DrawTriangle(parameters);
            }
        }
    }
}
