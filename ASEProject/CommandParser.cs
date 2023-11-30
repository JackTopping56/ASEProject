using System;
using System.Collections.Generic;
using System.Linq;

namespace ASEProject
{
    /// <summary>
    /// Exception thrown when an invalid command is encountered.
    /// </summary>
    public class InvalidCommandException : Exception
    {
        public InvalidCommandException() { }
        public InvalidCommandException(string message) : base(message) { }
        public InvalidCommandException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// A class responsible for parsing and validating commands in a drawing application.
    /// </summary>
    public class CommandParser
    {
        /// <summary>
        /// Gets a list of valid command keywords.
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
        /// Validates if a command is a valid keyword.
        /// </summary>
        /// <param name="command">The command to validate.</param>
        /// <returns>True if the command is a valid keyword; otherwise, false.</returns>
        public bool IsValidCommand(string command)
        {
            if (IsVariableDeclaration(command))
            {
                return true;
            }

            string[] parts = command.Split(' ');

            if (parts.Length == 0)
                throw new InvalidCommandException("Empty command.");

            string action = parts[0].ToLower();
            if (!ValidCommands.Contains(action))
                throw new InvalidCommandException($"Unknown command: {action}");

            return true;
        }

        /// <summary>
        /// Validates if a command has valid parameters.
        /// </summary>
        /// <param name="command">The command to validate.</param>
        /// <returns>True if the command has valid parameters; otherwise, false.</returns>
        public bool HasValidParameters(string command)
        {
            string[] parts = command.Split(' ');

            if (parts.Length == 1)
            {
                string cmd = parts[0].ToLower();
                if (cmd == "clear" || cmd == "reset")
                    return true;
            }
            else if (parts.Length < 2)
            {
                throw new InvalidCommandException("Command is missing parameters.");
            }

            string action = parts[0].ToLower();
            string[] parameters = parts.Skip(1).ToArray();

            switch (action)
            {
                case "moveto":
                    if (!IsValidMoveToParameters(parameters))
                        throw new InvalidCommandException("Invalid parameters for 'moveto' command.");
                    break;
                case "drawto":
                    if (!IsValidDrawToParameters(parameters))
                        throw new InvalidCommandException("Invalid parameters for 'drawto' command.");
                    break;
                case "rectangle":
                    if (!IsValidRectangleParameters(parameters))
                        throw new InvalidCommandException("Invalid parameters for 'rectangle' command.");
                    break;
                case "circle":
                    if (!IsValidCircleParameters(parameters))
                        throw new InvalidCommandException("Invalid parameters for 'circle' command.");
                    break;
                case "triangle":
                    if (!IsValidTriangleParameters(parameters))
                        throw new InvalidCommandException("Invalid parameters for 'triangle' command.");
                    break;
                case "pen":
                    if (!IsValidPenParameters(parameters))
                        throw new InvalidCommandException("Invalid parameters for 'pen' command.");
                    break;
                case "fill":
                    if (!IsValidFillParameters(parameters))
                        throw new InvalidCommandException("Invalid parameters for 'fill' command.");
                    break;
                default:
                    throw new InvalidCommandException($"Unknown command: {action}");
            }

            return true;
        }

        // Helper methods for specific command parameter validation...

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
            if (parameters.Length != 6)
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

        public bool IsVariableDeclaration(string command)
        {
            var parts = command.Split('=');
            if (parts.Length != 2)
            {
                return false;
            }

            var variableName = parts[0].Trim();
            var variableValue = parts[1].Trim();

            return IsValidVariableName(variableName) && int.TryParse(variableValue, out _);
        }

        // Helper method to validate variable names (simple implementation)
        private bool IsValidVariableName(string variableName)
        {
            return !string.IsNullOrEmpty(variableName) && variableName.All(char.IsLetter);
        }
    }
}
