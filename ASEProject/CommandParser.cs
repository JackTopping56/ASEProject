using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;


namespace ASEProject;
public class CommandParser
{
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

    public bool IsValidCommand(string command)
    {
        string[] parts = command.Split(' ');

        if (parts.Length == 0)
            return false;

        string action = parts[0].ToLower();
        return ValidCommands.Contains(action);
    }

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
