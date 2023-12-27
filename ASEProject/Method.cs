using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEProject
{
    public class Method
    {
        public string Name { get; set; }
        public List<string> Parameters { get; set; }
        public List<string> Commands { get; set; }

        public Method(string name)
        {
            Name = name;
            Parameters = new List<string>();
            Commands = new List<string>();
        }
    }
}

