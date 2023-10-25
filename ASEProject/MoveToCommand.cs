using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ASEProject
{
    public class MoveToCommand : ICommand
    {
        public void Execute(CommandList commandList, string[] parameters)
        {
            if (parameters.Length >= 3 && int.TryParse(parameters[1], out int x) && int.TryParse(parameters[2], out int y))
            {
                commandList.MoveTo(parameters);
            }
        }
    }

}
