using System;
using System.Collections.Generic;
using System.Linq;

namespace ASEProject
{
    /// <summary>
    /// Represents a class for parsing and validating commands in a graphics drawing application.
    /// </summary>
    public class CommandParser
    {
        /// <summary>
        /// Gets a list of valid commands that can be processed by the parser.
        /// </summary>
        public List<string> ValidCommands { get; } = new List<string>
        {
            "moveto",
            "drawto",
            "clear",
            "reset",
            "rectangle",
            "circle",
            "triangle",
            "pen",
            "fill"
        };

        /// <summary>
        /// Checks if a given command is a valid command recognized by the parser.
        /// </summary>
        /// <param name="command">The command to validate.</param>
        /// <returns>True if the command is valid; otherwise, false.</returns>
        public bool IsValidCommand(string command)
        {
            string[] parts = command.Split(' ');

            if (parts.Length == 0)
                return false;

            string action = parts[0].ToLower();
            return ValidCommands.Contains(action);
        }

        /// <summary>
        /// Checks if a command has valid parameters based on its action.
        /// </summary>
        /// <param name="command">The command to validate.</param>
        /// <returns>True if the parameters are valid; otherwise, false.</returns>
        public bool HasValidParameters(string command)
        {
            string[] parts = command.Split(' ');

            if (parts.Length < 2)
                return false;

            string action = parts[0].ToLower();
            string[] parameters = parts.Skip(1).ToArray();

            switch (action)
            {
                case "moveto":
                    return IsValidMoveToParameters(parameters);
                case "drawto":
                    return IsValidDrawToParameters(parameters);
                case "clear":
                case "reset":
                    return parameters.Length == 0;
                case "rectangle":
                    return IsValidRectangleParameters(parameters);
                case "circle":
                    return IsValidCircleParameters(parameters);
                case "triangle":
                    return IsValidTriangleParameters(parameters);
                case "pen":
                    return IsValidPenParameters(parameters);
                case "fill":
                    return IsValidFillParameters(parameters);
                default:
                    return false;
            }
        }

        // Other private helper methods for parameter validation...

        private bool IsValidMoveToParameters(string[] parameters)
        {
            if (parameters.Length != 2)
                return false;

            return int.TryParse(parameters[0], out _) && int.TryParse(parameters[1], out _);
        }

        private bool IsValidDrawToParameters(string[] parameters)
        {
            if (parameters.Length != 2)
                return false;

            return int.TryParse(parameters[0], out _) && int.TryParse(parameters[1], out _);
        }

        private bool IsValidRectangleParameters(string[] parameters)
        {
            if (parameters.Length != 2)
                return false;

            return int.TryParse(parameters[0], out _) && int.TryParse(parameters[1], out _);
        }

        private bool IsValidCircleParameters(string[] parameters)
        {
            if (parameters.Length != 1)
                return false;

            return int.TryParse(parameters[0], out _);
        }

        private bool IsValidTriangleParameters(string[] parameters)
        {
            if (parameters.Length != 4)
                return false;

            return parameters.All(param => int.TryParse(param, out _));
        }

        private bool IsValidPenParameters(string[] parameters)
        {
            if (parameters.Length != 1)
                return false;

            string color = parameters[0].ToLower();
            return color == "red" || color == "green" || color == "blue" || color == "black";
        }

        private bool IsValidFillParameters(string[] parameters)
        {
            if (parameters.Length != 1)
                return false;

            string fillMode = parameters[0].ToLower();
            return fillMode == "on" || fillMode == "off";
        }
    }
}
