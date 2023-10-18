using System;
using System.Drawing;
using Xunit;
using ASEProject; // Replace "YourNamespace" with the actual namespace of the CommandList class
using System.Reflection;

namespace CommandListTests
{
    public class CommandListTests
    {
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
            Assert.Equal(PointF.Empty, commandList.GetCurrentPosition());
        }

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
            Assert.Equal(PointF.Empty, commandList.GetCurrentPosition());
        }

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

    }
}