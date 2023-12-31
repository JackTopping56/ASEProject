﻿using ASEProject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

/// <summary>
/// Represents a class for executing a list of drawing commands using a Graphics object and a Pen.
/// </summary>
public class CommandList
{
    private Graphics graphics;
    private Pen pen;
    private PointF currentPosition;
    private Dictionary<string, int> userVariables;
    private Dictionary<string, Method> methods;

    /// <summary>
    /// Gets or sets the fill mode for shapes. When set to true, shapes are filled; when set to false, only outlines are drawn.
    /// </summary>
    internal bool FillModeOn { get; set; } = true;

    /// <summary>
    /// Initializes a new instance of the CommandList class with the specified Graphics object.
    /// </summary>
    /// <param name="graphics">The Graphics object to use for drawing.</param>
    public CommandList(Graphics graphics)
    {
        this.graphics = graphics;
        pen = new Pen(Color.Black);
        currentPosition = PointF.Empty;
        userVariables = new Dictionary<string, int>();
        methods = new Dictionary<string, Method>();
    }


    /// <summary>
    /// Gets the color of the current drawing pen.
    /// </summary>
    /// <returns>The color of the pen.</returns>
    public Color GetPenColor()
    {
        return pen.Color;
    }

    /// <summary>
    /// Executes the specified drawing command.
    /// </summary>
    /// <param name="command">A string representing the drawing command to execute.</param>
    public void ExecuteCommand(string command)
    {
        string[] parts = command.Split(' ');

        if (parts.Length < 2)
        {
            // Invalid command
            return;
        }

        string action = parts[0].ToLower();
        switch (action)
        {
            case "moveto":
                MoveTo(parts);
                break;
            case "drawto":
                DrawTo(parts);
                break;
            case "clear":
                Clear();
                break;
            case "reset":
                Reset();
                break;
            case "rectangle":
                DrawRectangle(parts);
                break;
            case "circle":
                DrawCircle(parts);
                break;
            case "triangle":
                DrawTriangle(parts);
                break;
            case "pen":
                ChangePenColor(parts);
                break;
            case "fill":
                ChangeFillMode(parts);
                break;
            default:
                // Unknown command
                break;
        }
    }

    /// <summary>
    /// Moves the drawing cursor to the specified coordinates.
    /// </summary>
    /// <param name="parts">An array containing the "moveto" command and X, Y coordinates.</param>
    public void MoveTo(string[] parts)
    {
        if (parts.Length >= 3)
        {
            int x = int.TryParse(parts[1], out var num) ? num : GetVariable(parts[1]);
            int y = int.TryParse(parts[2], out num) ? num : GetVariable(parts[2]);
            currentPosition = new PointF(x, y);
        }
    }

    /// <summary>
    /// Draws a line from the current position to the specified coordinates.
    /// </summary>
    /// <param name="parts">An array containing the "drawto" command and X, Y coordinates.</param>
    public void DrawTo(string[] parts)
    {
        if (parts.Length >= 3)
        {
            int x = int.TryParse(parts[1], out var num) ? num : GetVariable(parts[1]);
            int y = int.TryParse(parts[2], out num) ? num : GetVariable(parts[2]);
            PointF endPoint = new PointF(x, y);
            graphics.DrawLine(pen, currentPosition, endPoint);
            currentPosition = endPoint;
        }
    }

    /// <summary>
    /// Clears the drawing area and resets the current position to the origin (0,0).
    /// </summary>
    public void Clear()
    {
        if (graphics != null)
        {
           
            graphics.Clear(Color.DarkGray);
            currentPosition = PointF.Empty;
        }
    }

    /// <summary>
    /// Resets the current position to the origin (0,0).
    /// </summary>
    public void Reset()
    {
        currentPosition = PointF.Empty;
    }

    /// <summary>
    /// Draws a rectangle at the current position with the specified width and height.
    /// </summary>
    /// <param name="parts">An array containing the "rectangle" command and the width and height of the rectangle.</param>
    public void DrawRectangle(string[] parts)
    {
        if (parts.Length >= 3)
        {
            int width = int.TryParse(parts[1], out var num) ? num : GetVariable(parts[1]);
            int height = int.TryParse(parts[2], out num) ? num : GetVariable(parts[2]);
            RectangleF rect = new RectangleF(currentPosition.X, currentPosition.Y, width, height);
            if (FillModeOn)
            {
                graphics.FillRectangle(pen.Brush, rect);
            }
            graphics.DrawRectangle(pen, Rectangle.Round(rect));
        }
    }

    /// <summary>
    /// Draws a circle at the current position with the specified radius.
    /// </summary>
    /// <param name="parts">An array containing the "circle" command and the radius of the circle.</param>
    public void DrawCircle(string[] parts)
    {
        if (parts.Length >= 2)
        {
            int radius;

            // Debugging: Check if the part is a number or a variable
            if (int.TryParse(parts[1], out radius))
            {
                MessageBox.Show($"Using direct value for radius: {radius}", "Debug Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (userVariables.TryGetValue(parts[1], out radius))
            {
                MessageBox.Show($"Using variable '{parts[1]}' for radius, value: {radius}", "Debug Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Radius value or variable '{parts[1]}' not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Exit the method if radius not found
            }

            float diameter = radius * 2;
            RectangleF rect = new RectangleF(currentPosition.X - radius, currentPosition.Y - radius, diameter, diameter);

            // Drawing logic
            if (FillModeOn)
            {
                graphics.FillEllipse(pen.Brush, rect);
            }
            graphics.DrawEllipse(pen, rect);
        }
        else
        {
            MessageBox.Show("Invalid command format for 'circle'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }


    /// <summary>
    /// Draws a triangle at the current position with specified dimensions.
    /// </summary>
    /// <param name="parts">An array containing the "triangle" command and the width, height, and coordinates of the triangle.</param>
    public void DrawTriangle(string[] parts)
    {
        if (parts.Length >= 6)
        {
            int x1 = int.TryParse(parts[1], out var numX1) ? numX1 : GetVariable(parts[1]);
            int y1 = int.TryParse(parts[2], out var numY1) ? numY1 : GetVariable(parts[2]);
            int x2 = int.TryParse(parts[3], out var numX2) ? numX2 : GetVariable(parts[3]);
            int y2 = int.TryParse(parts[4], out var numY2) ? numY2 : GetVariable(parts[4]);
            int x3 = int.TryParse(parts[5], out var numX3) ? numX3 : GetVariable(parts[5]);
            int y3 = int.TryParse(parts[6], out var numY3) ? numY3 : GetVariable(parts[6]);

            PointF[] points = { new PointF(x1, y1), new PointF(x2, y2), new PointF(x3, y3) };
            if (FillModeOn)
            {
                graphics.FillPolygon(pen.Brush, points);
            }
            graphics.DrawPolygon(pen, points);
        }
    }

    /// <summary>
    /// Changes the color of the drawing pen based on the specified color name.
    /// </summary>
    /// <param name="parts">An array containing the "pen" command and the color name (e.g., "red", "green", "blue", "black").</param>
    public void ChangePenColor(string[] parts)
    {
        if (parts.Length >= 2)
        {
            string colorName = parts[1].ToLower();
            switch (colorName)
            {
                case "red":
                    pen.Color = Color.Red;
                    break;
                case "green":
                    pen.Color = Color.Green;
                    break;
                case "blue":
                    pen.Color = Color.Blue;
                    break;
                case "black":
                    pen.Color = Color.Black;
                    break;
                default:
                    // Invalid color name
                    break;
            }
        }
    }

    /// <summary>
    /// Changes the fill mode for shapes (on or off) based on the specified fill mode.
    /// </summary>
    /// <param name="parts">An array containing the "fill" command and the fill mode ("on" or "off").</param>
    public void ChangeFillMode(string[] parts)
    {
        if (parts.Length >= 2)
        {
            string fillMode = parts[1].ToLower();

            try
            {
                if (fillMode == "on")
                {
                    FillModeOn = true;
                }
                else if (fillMode == "off")
                {
                    FillModeOn = false;
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Error in ChangeFillMode: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Gets the current drawing position.
    /// </summary>
    /// <returns>The current drawing position as a PointF object.</returns>
    public PointF GetCurrentPosition()
    {
        return currentPosition;
    }

    /// <summary>
    /// Adds or updates a user-defined variable with the specified value.
    /// </summary>
    /// <param name="variableName">The name of the variable to add or update.</param>
    /// <param name="value">The value to assign to the variable.</param>
    public void SetVariable(string variableName, int value)
    {
        userVariables[variableName] = value;
    }

    /// <summary>
    /// Retrieves the value of a user-defined variable.
    /// </summary>
    /// <param name="variableName">The name of the variable to retrieve.</param>
    /// <returns>The value of the variable.</returns>
    public int GetVariable(string variableName)
    {
        if (userVariables.TryGetValue(variableName, out int value))
        {
            return value;
        }
        else
        {
            throw new KeyNotFoundException($"Variable '{variableName}' not found.");
        }
    }

    /// <summary>
    /// Displays the value of a user-defined variable in a message box.
    /// </summary>
    /// <param name="variableName">The name of the variable to display.</param>
    public void ShowVariableValue(string variableName)
    {
        if (userVariables.TryGetValue(variableName, out int value))
        {
            MessageBox.Show($"Variable '{variableName}' has value: {value}", "Variable Value", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show($"Variable '{variableName}' is not defined.", "Variable Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    /// <summary>
    /// Checks whether a user-defined variable exists.
    /// </summary>
    /// <param name="variableName">The name of the variable to check.</param>
    /// <returns>True if the variable exists; otherwise, false.</returns>
    public bool IsVariable(string variableName)
    {
        return userVariables.ContainsKey(variableName);
    }

    /// <summary>
    /// Adds a new method to the command list or updates an existing one.
    /// </summary>
    /// <param name="method">The method to add or update.</param>
    public void AddMethod(Method method)
    {
        methods[method.Name] = method;
    }

    /// <summary>
    /// Executes a specified method with the given parameters.
    /// </summary>
    /// <param name="methodName">The name of the method to execute.</param>
    /// <param name="parameters">A list of parameters to pass to the method.</param>
    /// <exception cref="Exception">Thrown if the method is not found or if the number of parameters is incorrect.</exception>
    public void ExecuteMethod(string methodName, List<string> parameters)
    {
        if (!methods.TryGetValue(methodName, out Method method))
        {
            throw new Exception($"Method '{methodName}' not found.");
        }

        if (parameters.Count != method.Parameters.Count)
        {
            throw new Exception($"Method '{methodName}' called with incorrect number of parameters.");
        }

        // Map actual parameters to method's parameters
        Dictionary<string, string> parameterMap = new Dictionary<string, string>();
        for (int i = 0; i < method.Parameters.Count; i++)
        {
            parameterMap[method.Parameters[i]] = parameters[i];
        }

        // Execute each command in the method
        foreach (var command in method.Commands)
        {
            string processedCommand = ReplaceParametersInCommand(command, parameterMap);
            ExecuteCommand(processedCommand);
        }
    }

    /// <summary>
    /// Replaces parameters in a command with their actual values.
    /// </summary>
    /// <param name="command">The command containing parameters to replace.</param>
    /// <param name="parameterMap">A dictionary mapping parameter names to their actual values.</param>
    /// <returns>The processed command with actual parameter values.</returns>
    private string ReplaceParametersInCommand(string command, Dictionary<string, string> parameterMap)
    {
        var parts = command.Split(' ');
        for (int i = 0; i < parts.Length; i++)
        {
            if (parameterMap.TryGetValue(parts[i], out string replacement))
            {
                parts[i] = replacement;
            }
        }
        return string.Join(" ", parts);
    }

    /// <summary>
    /// Checks if a method with the specified name exists in the command list.
    /// </summary>
    /// <param name="methodName">The name of the method to check.</param>
    /// <returns>True if the method exists; otherwise, false.</returns>
    public bool MethodExists(string methodName)
    {
        return methods.ContainsKey(methodName);
    }

    /// <summary>
    /// Retrieves a method by its name.
    /// </summary>
    /// <param name="methodName">The name of the method to retrieve.</param>
    /// <returns>The method with the specified name.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the method is not found.</exception>
    public Method GetMethod(string methodName)
    {
        if (methods.TryGetValue(methodName, out Method method))
        {
            return method;
        }
        else
        {
            throw new KeyNotFoundException($"Method '{methodName}' not found.");
        }
    }
}
