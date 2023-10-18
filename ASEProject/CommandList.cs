using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Drawing;
using System.Windows.Forms;


/// <summary>
/// Represents a command list for drawing shapes using <see cref="System.Drawing.Graphics"/>.
/// </summary>

public class CommandList
{
    private Graphics graphics;
    private Pen pen;
    private PointF currentPosition;
    private bool fillModeOn = true; // Initialize fill mode to "on"


    /// <summary>
    /// Initializes a new instance of the <see cref="CommandList"/> class with the specified <see cref="System.Drawing.Graphics"/>.
    /// </summary>
    /// <param name="graphics">The <see cref="System.Drawing.Graphics"/> object to draw on.</param>
    public CommandList(Graphics graphics)
    {
        this.graphics = graphics;
        pen = new Pen(Color.Black); // Default pen color
        currentPosition = PointF.Empty;
    }

    public CommandList()
    {
    }

    /// <summary>
    /// Executes a drawing command.
    /// </summary>
    /// <param name="command">The command to execute.</param>
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
    /// Moves the current position to the specified coordinates.
    /// </summary>
    /// <param name="parts">An array of command parts containing X and Y coordinates.</param>
    private void MoveTo(string[] parts)
    {
        if (parts.Length >= 3 && int.TryParse(parts[1], out int x) && int.TryParse(parts[2], out int y))
        {
            currentPosition = new PointF(x, y);
        }
    }

    /// <summary>
    /// Draws a line from the current position to the specified coordinates.
    /// </summary>
    /// <param name="parts">An array of command parts containing X and Y coordinates.</param>
    private void DrawTo(string[] parts)
    {
        if (parts.Length >= 3 && int.TryParse(parts[1], out int x) && int.TryParse(parts[2], out int y))
        {
            PointF endPoint = new PointF(x, y);
            graphics.DrawLine(pen, currentPosition, endPoint);
            currentPosition = endPoint;
        }
    }

    /// <summary>
    /// Clears the graphics and resets the current position.
    /// </summary>
    public void Clear()
    {
        Console.WriteLine("Clear method called"); // Debug output
        graphics.Clear(Color.White);
        currentPosition = PointF.Empty;
    }

    /// <summary>
    /// Resets the current position to (0, 0).
    /// </summary>
    public void Reset()
    {
        currentPosition = PointF.Empty;
    }

    /// <summary>
    /// Draws a rectangle with the specified width and height from the current position.
    /// </summary>
    /// <param name="parts">An array of command parts containing width and height.</param>
    private void DrawRectangle(string[] parts)
    {
        if (parts.Length >= 3 && int.TryParse(parts[1], out int width) && int.TryParse(parts[2], out int height))
        {
            RectangleF rect = new RectangleF(currentPosition.X, currentPosition.Y, width, height);
            if (fillModeOn)
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
    /// Draws a circle with the specified radius from the current position.
    /// </summary>
    /// <param name="parts">An array of command parts containing the radius.</param>
    private void DrawCircle(string[] parts)
    {
        if (parts.Length >= 2 && int.TryParse(parts[1], out int radius))
        {
            float diameter = radius * 2;
            RectangleF rect = new RectangleF(currentPosition.X, currentPosition.Y, diameter, diameter);
            if (fillModeOn)
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
    /// Draws a triangle with the specified coordinates.
    /// </summary>
    /// <param name="parts">An array of command parts containing X and Y coordinates for the triangle's vertices.</param>
    public void DrawTriangle(string[] parts)
    {
        if (parts.Length >= 6 && int.TryParse(parts[1], out int width) && int.TryParse(parts[2], out int height)
            && int.TryParse(parts[3], out int x2) && int.TryParse(parts[4], out int y2))
        {
            int x1 = x2 - width; // Calculate the x-coordinate of the top-left corner
            int y1 = y2 - height; // Calculate the y-coordinate of the top-left corner

            PointF[] trianglePoints = new PointF[]
            {
            new PointF(x1, y2), // Top-left corner
            new PointF(x2, y2), // Top-right corner
            new PointF(x2 - width / 2, y1) // Bottom corner
            };

            if (fillModeOn)
            {
                if (pen.Brush != null && pen.Brush != Brushes.Transparent)
                {
                    graphics.FillPolygon(pen.Brush, trianglePoints); // Fill the triangle
                }
            }
            graphics.DrawPolygon(pen, trianglePoints); // Draw the outline
        }
    }


    /// <summary>
    /// Changes the pen color.
    /// </summary>
    /// <param name="parts">An array of command parts containing the new color name.</param>
    private void ChangePenColor(string[] parts)
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
    /// Changes the fill mode to "on" or "off."
    /// </summary>
    /// <param name="parts">An array of command parts containing the fill mode ("on" or "off").</param>
    private void ChangeFillMode(string[] parts)
    {
        if (parts.Length >= 2)
        {
            string fillMode = parts[1].ToLower();

            try
            {
                if (fillMode == "on")
                {
                    fillModeOn = true; // Set fill mode to "on"
                }
                else if (fillMode == "off")
                {
                    fillModeOn = false; // Set fill mode to "off"
                }
            }
            catch (ArgumentNullException ex)
            {
                // Handle the exception, e.g., log it or show an error message
                Console.WriteLine($"Error in ChangeFillMode: {ex.Message}");
            }
        }
    }

    public IEnumerable<object> GetCurrentPosition()
    {
        throw new NotImplementedException();
    }
}
