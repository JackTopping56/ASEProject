using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEProject
{

    public class ChangePenColorCommand : ICommand
    {
        public void Execute(CommandList commandList, string[] parameters)
        {
            if (parameters.Length >= 2)
            {
                commandList.ChangePenColor(parameters);
            }
        }
    }
}
