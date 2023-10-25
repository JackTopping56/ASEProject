using System;
using System.Collections.Generic;
using System.Drawing;

/// <summary>
/// Represents a class for executing a list of drawing commands using a Graphics object and a Pen.
/// </summary>
public class CommandList
{
    private Graphics graphics;
    private Pen pen;
    private PointF currentPosition;

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
    }

    /// <summary>
    /// Initializes a new instance of the CommandList class without a Graphics object.
    /// </summary>
    public CommandList()
    {
        // You can add any initialization logic here if needed.
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
        if (parts.Length >= 3 && int.TryParse(parts[1], out int x) && int.TryParse(parts[2], out int y))
        {
            currentPosition = new PointF(x, y);
        }
    }

    /// <summary>
    /// Draws a line from the current position to the specified coordinates.
    /// </summary>
    /// <param name="parts">An array containing the "drawto" command and X, Y coordinates.</param>
    public void DrawTo(string[] parts)
    {
        if (parts.Length >= 3 && int.TryParse(parts[1], out int x) && int.TryParse(parts[2], out int y))
        {
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
        graphics.Clear(Color.DarkGray);
        currentPosition = PointF.Empty;
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
        if (parts.Length >= 3 && int.TryParse(parts[1], out int width) && int.TryParse(parts[2], out int height))
        {
            RectangleF rect = new RectangleF(currentPosition.X, currentPosition.Y, width, height);
            if (FillModeOn)
            {
                if (pen.Brush != null && pen.Brush != Brushes.Transparent)
                {
                    graphics.FillRectangle(pen.Brush, rect);
                }
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
        if (parts.Length >= 2 && int.TryParse(parts[1], out int radius))
        {
            float diameter = radius * 2;
            RectangleF rect = new RectangleF(currentPosition.X, currentPosition.Y, diameter, diameter);
            if (FillModeOn)
            {
                if (pen.Brush != null && pen.Brush != Brushes.Transparent)
                {
                    graphics.FillEllipse(pen.Brush, rect);
                }
            }
            graphics.DrawEllipse(pen, Rectangle.Round(rect));
        }
    }

    /// <summary>
    /// Draws a triangle at the current position with specified dimensions.
    /// </summary>
    /// <param name="parts">An array containing the "triangle" command and the width, height, and coordinates of the triangle.</param>
    public void DrawTriangle(string[] parts)
    {
        if (parts.Length >= 6 && int.TryParse(parts[1], out int width) && int.TryParse(parts[2], out int height)
            && int.TryParse(parts[3], out int x2) && int.TryParse(parts[4], out int y2))
        {
            int x1 = x2 - width;
            int y1 = y2 - height;

            PointF[] trianglePoints = new PointF[]
            {
                new PointF(x1, y2),
                new PointF(x2, y2),
                new PointF(x2 - width / 2, y1)
            };

            if (FillModeOn)
            {
                if (pen.Brush != null && pen.Brush != Brushes.Transparent)
                {
                    graphics.FillPolygon(pen.Brush, trianglePoints);
                }
            }
            graphics.DrawPolygon(pen, trianglePoints);
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
}
