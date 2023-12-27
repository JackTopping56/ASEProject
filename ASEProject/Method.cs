using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEProject
{
    /// <summary>
    /// Represents a method in the drawing application. 
    /// A method is defined with a name, a list of parameters, and a list of commands to execute.
    /// </summary>
    public class Method
    {
        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        /// <value>The name of the method.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the list of parameters used by the method. These parameters can be used within the commands of the method.
        /// </summary>
        /// <value>A list of parameter names.</value>
        public List<string> Parameters { get; set; }


        /// <summary>
        /// Gets the list of commands that the method will execute. These commands are executed in the order they appear in the list.
        /// </summary>
        /// <value>A list of drawing commands.</value>
        public List<string> Commands { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Method"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the method.</param>
        public Method(string name)
        {
            Name = name;
            Parameters = new List<string>();
            Commands = new List<string>();
        }
    }
}

