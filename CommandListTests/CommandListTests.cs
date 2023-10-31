using System;
using System.Drawing;
using Xunit;
using ASEProject; // Replace "YourNamespace" with the actual namespace of the CommandList class
using System.Reflection;

/// <summary>
/// This XML documentation provides detailed information about the CommandListTests class, 
/// which contains a set of unit tests for the CommandList class in the ASEProject namespace.
/// </summary>

namespace CommandListTests
{
    public class CommandListTests
    {
        /// <summary>
        /// Verifies that the ExecuteCommand method correctly handles the "moveto" command.
        /// </summary>
        [Fact]
        public void ExecuteCommand_ValidMoveTo()
        {
            // Arrange
            var graphics = Graphics.FromImage(new Bitmap(1, 1));
            var commandList = new CommandList(graphics);

            // Act
            commandList.ExecuteCommand("moveto 100 100");

            // Assert
            Assert.Equal(new PointF(100, 100), commandList.GetCurrentPosition());
        }

        /// <summary>
        /// Verifies that the ExecuteCommand method correctly handles the "drawto" command.
        /// </summary>
        [Fact]
        public void ExecuteCommand_ValidDrawTo()
        {
            // Arrange
            var graphics = Graphics.FromImage(new Bitmap(1, 1));
            var commandList = new CommandList(graphics);

            // Act
            commandList.ExecuteCommand("moveto 50 50");
            commandList.ExecuteCommand("drawto 100 100");

            // Assert
            Assert.Equal(new PointF(100, 100), commandList.GetCurrentPosition());
        }

        /// <summary>
        /// Verifies that the ExecuteCommand method correctly handles the "clear" command.
        /// </summary>
        [Fact]
        public void ExecuteCommand_Clear()
        {
            // Arrange
            var graphics = Graphics.FromImage(new Bitmap(1, 1));
            var commandList = new CommandList(graphics);

            // Act
            commandList.ExecuteCommand("moveto 50 50");
            commandList.ExecuteCommand("clear");

            // Assert
            Assert.Equal(new PointF(50,50), commandList.GetCurrentPosition());
        }

        /// <summary>
        /// Verifies that the ExecuteCommand method correctly handles the "reset" command.
        /// </summary>
        [Fact]
        public void ExecuteCommand_Reset()
        {
            // Arrange
            var graphics = Graphics.FromImage(new Bitmap(1, 1));
            var commandList = new CommandList(graphics);

            // Act
            commandList.ExecuteCommand("moveto 50 50");
            commandList.ExecuteCommand("reset");

            // Assert
            Assert.Equal(new PointF(50,50), commandList.GetCurrentPosition());
        }

        /// <summary>
        /// Verifies that the ExecuteCommand method correctly handles the "rectangle" command.
        /// </summary>
        [Fact]
        public void ExecuteCommand_ValidRectangle()
        {
            // Arrange
            var graphics = Graphics.FromImage(new Bitmap(1, 1));
            var commandList = new CommandList(graphics);

            // Act
            commandList.ExecuteCommand("moveto 50 50");
            commandList.ExecuteCommand("rectangle 50 50");

            // Assert (You may add more specific assertions for rectangle drawing)
            Assert.NotEqual(PointF.Empty, commandList.GetCurrentPosition());
        }

        /// <summary>
        /// Verifies that the ExecuteCommand method correctly handles the "circle" command.
        /// </summary>
        [Fact]
        public void ExecuteCommand_ValidCircle()
        {
            // Arrange
            var graphics = Graphics.FromImage(new Bitmap(1, 1));
            var commandList = new CommandList(graphics);

            // Act
            commandList.ExecuteCommand("moveto 50 50");
            commandList.ExecuteCommand("circle 25");

            // Assert (You may add more specific assertions for circle drawing)
            Assert.NotEqual(PointF.Empty, commandList.GetCurrentPosition());
        }

        /// <summary>
        /// Verifies that the ExecuteCommand method correctly handles the "triangle" command.
        /// </summary>
        [Fact]
        public void ExecuteCommand_ValidTriangle()
        {
            // Arrange
            var graphics = Graphics.FromImage(new Bitmap(1, 1));
            var commandList = new CommandList(graphics);

            // Act
            commandList.ExecuteCommand("moveto 50 50");
            commandList.ExecuteCommand("triangle 20 30 40 50 60 70");

            // Assert (You may add more specific assertions for triangle drawing)
            Assert.NotEqual(PointF.Empty, commandList.GetCurrentPosition());
        }

        /// <summary>
        /// Verifies that the ExecuteCommand method correctly handles an invalid command.
        /// </summary>
        [Fact]
        public void ExecuteCommand_InvalidCommand()
        {
            // Arrange
            var graphics = Graphics.FromImage(new Bitmap(1, 1));
            var commandList = new CommandList(graphics);

            // Act
            commandList.ExecuteCommand("invalidcommand");

            // Assert (You may add assertions for handling invalid commands)
            Assert.Equal(PointF.Empty, commandList.GetCurrentPosition());
        }

        /// <summary>
        /// Verifies that the ChangeFillMode method correctly turns the fill mode on.
        /// </summary>
        [Fact]
        public void ChangeFillMode_FillOn()
        {
            // Arrange
            var graphics = Graphics.FromImage(new Bitmap(1, 1));
            var commandList = new CommandList(graphics);

            // Act
            commandList.ExecuteCommand("fill on");

            // Use reflection to access FillModeOn property
            PropertyInfo fillModeOnProperty = commandList.GetType().GetProperty("FillModeOn", BindingFlags.NonPublic | BindingFlags.Instance);
            bool fillModeOnValue = (bool)fillModeOnProperty.GetValue(commandList);

            // Assert
            Assert.True(fillModeOnValue);
        }

        /// <summary>
        /// Verifies that the ChangeFillMode method correctly turns the fill mode off.
        /// </summary>
        [Fact]
        public void ChangeFillMode_FillOff()
        {
            // Arrange
            var graphics = Graphics.FromImage(new Bitmap(1, 1));
            var commandList = new CommandList(graphics);
            commandList.ExecuteCommand("fill on"); // Ensure fill mode is initially on

            // Act
            commandList.ExecuteCommand("fill off");

            // Use reflection to access FillModeOn property
            PropertyInfo fillModeOnProperty = commandList.GetType().GetProperty("FillModeOn", BindingFlags.NonPublic | BindingFlags.Instance);
            bool fillModeOnValue = (bool)fillModeOnProperty.GetValue(commandList);

            // Assert
            Assert.False(fillModeOnValue);
        }
        [Fact]
        public void ExecuteCommand_PenColorChange_Red()
        {
            // Arrange
            var graphics = Graphics.FromImage(new Bitmap(1, 1));
            var commandList = new CommandList(graphics);

            // Act
            commandList.ExecuteCommand("pen red");

            // Get the current pen color
            Color penColor = commandList.GetPenColor();

            // Assert
            Assert.Equal(Color.Red, penColor);
        }
    }
}