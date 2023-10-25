using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEProject
{
    public class ClearCommand : ICommand
    {
        public void Execute(CommandList commandList, string[] parameters)
        {
            commandList.Clear();
        }
    }
}
