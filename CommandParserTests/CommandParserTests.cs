using System;
using Xunit;
using ASEProject;

/// <summary>
/// Test class for the CommandParser class.
/// </summary>
public class CommandParserTests
{
    /// <summary>
    /// Tests the IsValidCommand method with a valid command.
    /// </summary>
    [Fact]
    public void IsValidCommand_ValidCommand_ReturnsTrue()
    {
        CommandParser parser = new CommandParser();
        string validCommand = "moveto 10 20";
        bool isValid = parser.IsValidCommand(validCommand);

        // Assert that the IsValidCommand method returns true for a valid command.
        Assert.True(isValid);
    }

    /// <summary>
    /// Tests the IsValidCommand method with an invalid command.
    /// </summary>
    [Fact]
    public void IsValidCommand_InvalidCommand_ReturnsFalse()
    {
        CommandParser parser = new CommandParser();
        string invalidCommand = "invalidcommand 10 20";
        bool isValid = parser.IsValidCommand(invalidCommand);

        // Assert that the IsValidCommand method returns false for an invalid command.
        Assert.False(isValid);
    }

    /// <summary>
    /// Tests the HasValidParameters method with valid parameters.
    /// </summary>
    [Fact]
    public void HasValidParameters_ValidParameters_ReturnsTrue()
    {
        CommandParser parser = new CommandParser();
        string validCommand = "circle 30";
        bool hasValidParameters = parser.HasValidParameters(validCommand);

        // Assert that the HasValidParameters method returns true for valid parameters.
        Assert.True(hasValidParameters);
    }

    /// <summary>
    /// Tests the HasValidParameters method with invalid parameters.
    /// </summary>
    [Fact]
    public void HasValidParameters_InvalidParameters_ReturnsFalse()
    {
        CommandParser parser = new CommandParser();
        string invalidCommand = "rectangle abc def";
        bool hasValidParameters = parser.HasValidParameters(invalidCommand);

        // Assert that the HasValidParameters method returns false for invalid parameters.
        Assert.False(hasValidParameters);
    }
}
