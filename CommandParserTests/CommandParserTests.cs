using System;
using Xunit;
using ASEProject;

public class CommandParserTests
{
    [Fact]
    public void IsValidCommand_ValidCommand_ReturnsTrue()
    {
        CommandParser parser = new CommandParser();
        string validCommand = "moveto 10 20";
        bool isValid = parser.IsValidCommand(validCommand);

        Assert.True(isValid);
    }

    [Fact]
    public void IsValidCommand_InvalidCommand_ReturnsFalse()
    {
        CommandParser parser = new CommandParser();
        string invalidCommand = "invalidcommand 10 20";

        try
        {
            bool isValid = parser.IsValidCommand(invalidCommand);
            Assert.False(isValid, "Expected the command to be invalid");
        }
        catch (InvalidCommandException)
        {
            // The exception was thrown as expected for an invalid command.
        }
    }

    [Fact]
    public void HasValidParameters_ValidParameters_ReturnsTrue()
    {
        CommandParser parser = new CommandParser();
        string validCommand = "circle 30";
        bool hasValidParameters = parser.HasValidParameters(validCommand);

        Assert.True(hasValidParameters);
    }

    [Fact]
    public void HasValidParameters_InvalidParameters_ReturnsFalse()
    {
        CommandParser parser = new CommandParser();
        string invalidCommand = "rectangle abc def";

        try
        {
            bool hasValidParameters = parser.HasValidParameters(invalidCommand);
            Assert.False(hasValidParameters, "Expected the parameters to be invalid");
        }
        catch (InvalidCommandException)
        {
            // The exception was thrown as expected for invalid parameters.
        }
    }
}
