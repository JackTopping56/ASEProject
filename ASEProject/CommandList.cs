using System;
using System.Collections.Generic;
using System.Drawing;

public class CommandList
{
    private Graphics graphics;
    private Pen pen;
    private PointF currentPosition;
    internal bool fillModeOn { get; set; } = true;

    public CommandList(Graphics graphics)
    {
        this.graphics = graphics;
        pen = new Pen(Color.Black);
        currentPosition = PointF.Empty;
    }

    public CommandList()
    {
    }

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

    private void MoveTo(string[] parts)
    {
        if (parts.Length >= 3 && int.TryParse(parts[1], out int x) && int.TryParse(parts[2], out int y))
        {
            currentPosition = new PointF(x, y);
        }
    }

    private void DrawTo(string[] parts)
    {
        if (parts.Length >= 3 && int.TryParse(parts[1], out int x) && int.TryParse(parts[2], out int y))
        {
            PointF endPoint = new PointF(x, y);
            graphics.DrawLine(pen, currentPosition, endPoint);
            currentPosition = endPoint;
        }
    }

    public void Clear()
    {
        graphics.Clear(Color.White);
        currentPosition = PointF.Empty;
    }

    public void Reset()
    {
        currentPosition = PointF.Empty;
    }

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

            if (fillModeOn)
            {
                if (pen.Brush != null && pen.Brush != Brushes.Transparent)
                {
                    graphics.FillPolygon(pen.Brush, trianglePoints);
                }
            }
            graphics.DrawPolygon(pen, trianglePoints);
        }
    }

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

    private void ChangeFillMode(string[] parts)
    {
        if (parts.Length >= 2)
        {
            string fillMode = parts[1].ToLower();

            try
            {
                if (fillMode == "on")
                {
                    fillModeOn = true;
                }
                else if (fillMode == "off")
                {
                    fillModeOn = false;
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Error in ChangeFillMode: {ex.Message}");
            }
        }
    }

    public PointF GetCurrentPosition()
    {
        return currentPosition;
    }
}
