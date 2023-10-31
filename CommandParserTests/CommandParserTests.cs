using System;
using Xunit;
using ASEProject;

/// <summary>
/// Unit tests for the <see cref="CommandParser"/> class.
/// </summary>
public class CommandParserTests
{
    /// <summary>
    /// Verifies that <see cref="CommandParser.IsValidCommand(string)"/> correctly identifies a valid command.
    /// </summary>
    [Fact]
    public void IsValidCommand_ValidCommand_ReturnsTrue()
    {
        CommandParser parser = new CommandParser();
        string validCommand = "moveto 10 20";
        bool isValid = parser.IsValidCommand(validCommand);

        Assert.True(isValid);
    }

    /// <summary>
    /// Verifies that <see cref="CommandParser.IsValidCommand(string)"/> correctly identifies an invalid command and throws an <see cref="InvalidCommandException"/>.
    /// </summary>
    [Fact]
    public void IsValidCommand_InvalidCommand_ReturnsFalse()
    {
        CommandParser parser = new CommandParser();
        string invalidCommand = "invalidcommand 10 20";

        Assert.Throws<InvalidCommandException>(() =>
        {
            parser.IsValidCommand(invalidCommand);
        });
    }

    /// <summary>
    /// Verifies that <see cref="CommandParser.HasValidParameters(string)"/> correctly identifies valid parameters for a command.
    /// </summary>
    [Fact]
    public void HasValidParameters_ValidParameters_ReturnsTrue()
    {
        CommandParser parser = new CommandParser();
        string validCommand = "circle 30";
        bool hasValidParameters = parser.HasValidParameters(validCommand);

        Assert.True(hasValidParameters);
    }

    /// <summary>
    /// Verifies that <see cref="CommandParser.HasValidParameters(string)"/> correctly identifies invalid parameters for a command and throws an <see cref="InvalidCommandException"/>.
    /// </summary>
    [Fact]
    public void HasValidParameters_InvalidParameters_ReturnsFalse()
    {
        CommandParser parser = new CommandParser();
        string invalidCommand = "rectangle abc def";

        Assert.Throws<InvalidCommandException>(() =>
        {
            parser.HasValidParameters(invalidCommand);
        });
    }
}
