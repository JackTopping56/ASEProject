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
            "fill", 
            "if", 
            "endif"
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

        /// <summary>
        /// Validates parameters for the "moveto" command.
        /// </summary>
        /// <param name="parameters">Array of parameters for the "moveto" command.</param>
        /// <returns>True if parameters are valid; otherwise, false.</returns>
        private bool IsValidMoveToParameters(string[] parameters)
        {
            if (parameters.Length != 2)
                return false;

            return int.TryParse(parameters[0], out _) && int.TryParse(parameters[1], out _);
        }

        /// <summary>
        /// Validates parameters for the "drawto" command.
        /// </summary>
        /// <param name="parameters">Array of parameters for the "drawto" command.</param>
        /// <returns>True if parameters are valid; otherwise, false.</returns>
        private bool IsValidDrawToParameters(string[] parameters)
        {
            if (parameters.Length != 2)
                return false;

            return int.TryParse(parameters[0], out _) && int.TryParse(parameters[1], out _);
        }

        /// <summary>
        /// Validates parameters for the "rectangle" command.
        /// </summary>
        /// <param name="parameters">Array of parameters for the "rectangle" command.</param>
        /// <returns>True if parameters are valid; otherwise, false.</returns>
        private bool IsValidRectangleParameters(string[] parameters)
        {
            if (parameters.Length != 2)
                return false;

            return int.TryParse(parameters[0], out _) && int.TryParse(parameters[1], out _);
        }

        /// <summary>
        /// Validates parameters for the "circle" command.
        /// </summary>
        /// <param name="parameters">Array of parameters for the "circle" command.</param>
        /// <returns>True if parameters are valid; otherwise, false.</returns>
        private bool IsValidCircleParameters(string[] parameters)
        {
            if (parameters.Length != 1)
                return false;

            return int.TryParse(parameters[0], out _);
        }

        /// <summary>
        /// Validates parameters for the "triangle" command.
        /// </summary>
        /// <param name="parameters">Array of parameters for the "triangle" command.</param>
        /// <returns>True if parameters are valid; otherwise, false.</returns>
        private bool IsValidTriangleParameters(string[] parameters)
        {
            if (parameters.Length != 6)
                return false;

            return parameters.All(param => int.TryParse(param, out _));
        }

        /// <summary>
        /// Validates parameters for the "pen" command.
        /// </summary>
        /// <param name="parameters">Array of parameters for the "pen" command.</param>
        /// <returns>True if parameters are valid; otherwise, false.</returns>
        private bool IsValidPenParameters(string[] parameters)
        {
            if (parameters.Length != 1)
                return false;

            string color = parameters[0].ToLower();
            return color == "red" || color == "green" || color == "blue" || color == "black";
        }

        /// <summary>
        /// Validates parameters for the "fill" command.
        /// </summary>
        /// <param name="parameters">Array of parameters for the "fill" command.</param>
        /// <returns>True if parameters are valid; otherwise, false.</returns>
        private bool IsValidFillParameters(string[] parameters)
        {
            if (parameters.Length != 1)
                return false;

            string fillMode = parameters[0].ToLower();
            return fillMode == "on" || fillMode == "off";
        }

        /// <summary>
        /// Checks if a command is a variable declaration.
        /// </summary>
        /// <param name="command">The command to check.</param>
        /// <returns>True if the command is a variable declaration; otherwise, false.</returns>
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

        /// <summary>
        /// Validates a variable name.
        /// </summary>
        /// <param name="variableName">The variable name to validate.</param>
        /// <returns>True if the variable name is valid; otherwise, false.</returns>
        private bool IsValidVariableName(string variableName)
        {
            return !string.IsNullOrEmpty(variableName) && variableName.All(char.IsLetter);
        }

        /// <summary>
        /// Replaces variables in a command with their corresponding values.
        /// </summary>
        /// <param name="command">The command string with variables.</param>
        /// <param name="commandList">The CommandList containing the variable values.</param>
        /// <returns>A command string with variables replaced by their values.</returns>
        public string ReplaceVariables(string command, CommandList commandList)
        {
            var parts = command.Split(' ');
            for (int i = 0; i < parts.Length; i++)
            {
                if (commandList.IsVariable(parts[i]))
                {
                    parts[i] = commandList.GetVariable(parts[i]).ToString();
                }
            }
            return string.Join(" ", parts);
        }

        /// <summary>
        /// Checks if a command is a conditional command (if or endif).
        /// </summary>
        /// <param name="command">The command to check.</param>
        /// <returns>True if the command is a conditional command; otherwise, false.</returns>
        public bool IsConditionalCommand(string command)
        {
            string[] parts = command.Split(' ');
            return parts.Length > 0 && (parts[0].ToLower() == "if" || parts[0].ToLower() == "endif");
        }

        /// <summary>
        /// Validates the syntax of a conditional command.
        /// </summary>
        /// <param name="command">The conditional command to validate.</param>
        /// <param name="commandList">The CommandList for variable checking.</param>
        /// <returns>True if the conditional command syntax is valid; otherwise, false.</returns>
        public bool IsValidConditionalCommand(string command, CommandList commandList)
        {
            string[] parts = command.Split(' ');
            if (parts[0].ToLower() == "if")
            {
                // Basic structure check: if variable operator value
                if (parts.Length != 4) return false;

                string variable = parts[1];
                string operation = parts[2]; 
                string value = parts[3];

                // Check if the variable exists
                if (!commandList.IsVariable(variable)) return false;

                // Validate the operation
                if (!(operation == ">" || operation == "<" || operation == "==")) return false;

                // Check if value is a number
                return int.TryParse(value, out _);
            }
            else if (parts[0].ToLower() == "endif")
            {
                // "endif" should be standalone
                return parts.Length == 1;
            }
            return false;
        }

        /// <summary>
        /// Checks if a command is related to loops (loop or endloop).
        /// </summary>
        /// <param name="command">The command to check.</param>
        /// <returns>True if the command is related to loops; otherwise, false.</returns>
        public bool IsLoopCommand(string command)
        {
            string[] parts = command.Split(' ');
            return parts.Length > 0 && (parts[0].ToLower() == "loop" || parts[0].ToLower() == "endloop");
        }

        /// <summary>
        /// Checks if a command is a variable declaration or involves arithmetic operations.
        /// </summary>
        /// <param name="command">The command to check.</param>
        /// <returns>True if the command is a variable declaration or an arithmetic operation; otherwise, false.</returns>
        public bool IsVariableDeclarationOrArithmetic(string command)
        {
            var parts = command.Split('=');
            if (parts.Length != 2)
            {
                return false;
            }

            var variableName = parts[0].Trim();
            var rightHandSide = parts[1].Trim();

            // If the right-hand side is just a number or variable, it's a simple declaration
            if (int.TryParse(rightHandSide, out _) || IsValidVariableName(rightHandSide))
            {
                return IsValidVariableName(variableName);
            }
            // If the right-hand side contains arithmetic operators, it's an arithmetic expression
            else
            {
                return IsValidVariableName(variableName) && IsArithmeticExpression(rightHandSide);
            }
        }

        /// <summary>
        /// Checks if a string is an arithmetic expression.
        /// </summary>
        /// <param name="expression">The string to check.</param>
        /// <returns>True if the string is an arithmetic expression; otherwise, false.</returns>
        private bool IsArithmeticExpression(string expression)
        {
            // This is a simple check for arithmetic expressions involving addition and subtraction
            char[] operators = new[] { '+', '-' };
            string[] parts = expression.Split(operators, StringSplitOptions.RemoveEmptyEntries);

            // Check if each part is either a number or a valid variable name.
            foreach (var part in parts)
            {
                string trimmedPart = part.Trim();
                if (!int.TryParse(trimmedPart, out _) && !IsValidVariableName(trimmedPart))
                {
                    return false;
                }
            }
            return parts.Length > 1; // There should be at least two parts for a valid expression
        }

        /// <summary>
        /// Determines whether a given command is a method definition.
        /// </summary>
        /// <param name="command">The command to check.</param>
        /// <returns>True if the command starts with 'method'; otherwise, false.</returns>
        public bool IsMethodDefinition(string command)
        {
            return command.StartsWith("method ");
        }

        /// <summary>
        /// Determines whether a given command is a call to a defined method.
        /// </summary>
        /// <param name="command">The command to check.</param>
        /// <returns>True if the command is formatted as a method call; otherwise, false.</returns>
        public bool IsMethodCall(string command)
        {
            var parts = command.Split('(');
            if (parts.Length != 2) return false;
            return IsValidMethodName(parts[0].Trim());
        }

        /// <summary>
        /// Checks if the provided string is a valid method name.
        /// </summary>
        /// <param name="methodName">The method name to validate.</param>
        /// <returns>True if the method name is not null or empty and contains only letters; otherwise, false.</returns>
        private bool IsValidMethodName(string methodName)
        {
           
            return !string.IsNullOrEmpty(methodName) && methodName.All(char.IsLetter);
        }

        /// <summary>
        /// Parses a list of strings representing a method definition and creates a Method object.
        /// </summary>
        /// <param name="methodLines">The lines of the method definition.</param>
        /// <returns>A new Method object based on the provided definition.</returns>
        /// <exception cref="InvalidCommandException">Thrown if the method definition is invalid.</exception>
        public Method ParseMethodDefinition(List<string> methodLines)
        {
            if (methodLines.Count == 0 || !IsMethodDefinition(methodLines[0]))
            {
                throw new InvalidCommandException("Invalid method definition.");
            }

            var headerParts = methodLines[0].Split(new[] { ' ', '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
            if (headerParts.Length < 2 || headerParts[0] != "method")
            {
                throw new InvalidCommandException("Invalid method header.");
            }

            string methodName = headerParts[1];
            var parameters = headerParts.Skip(2).ToList();
            var commands = methodLines.Skip(1).TakeWhile(line => !line.Trim().Equals("endmethod")).ToList();

            return new Method(methodName) { Parameters = parameters, Commands = commands };
        }

        /// <summary>
        /// Validates if a command string represents a valid method call based on existing methods.
        /// </summary>
        /// <param name="command">The command to validate.</param>
        /// <param name="commandList">The command list containing defined methods.</param>
        /// <returns>True if the command represents a valid method call; otherwise, false.</returns>
        public bool IsValidMethodCall(string command, CommandList commandList)
        {
            var parts = command.Split(new[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2 || !commandList.MethodExists(parts[0].Trim()))
            {
                return false;
            }

            var methodName = parts[0].Trim();
            var methodParams = parts[1].Split(',');
            var method = commandList.GetMethod(methodName);

            // Check if the number of parameters matches
            return methodParams.Length == method.Parameters.Count;
        }
    }
}
