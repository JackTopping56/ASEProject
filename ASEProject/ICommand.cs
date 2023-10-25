using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEProject
{
    using System;

    public interface ICommand
    {
        void Execute(CommandList commandList, string[] parameters);
    }

}
