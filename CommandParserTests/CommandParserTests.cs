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

    // Testing conditional command validation
    [Fact]
    public void IsConditionalCommand_ValidIf_ReturnsTrue()
    {
        CommandParser parser = new CommandParser();
        string ifCommand = "if x > 10";

        Assert.True(parser.IsConditionalCommand(ifCommand));
    }

    // Testing loop command validation
    [Fact]
    public void IsLoopCommand_ValidLoop_ReturnsTrue()
    {
        CommandParser parser = new CommandParser();
        string loopCommand = "loop 5";

        Assert.True(parser.IsLoopCommand(loopCommand));
    }

    // Testing variable declaration handling
    [Fact]
    public void IsVariableDeclaration_ValidDeclaration_ReturnsTrue()
    {
        CommandParser parser = new CommandParser();
        string varDeclaration = "x = 10";

        Assert.True(parser.IsVariableDeclarationOrArithmetic(varDeclaration));
    }

    // Testing arithmetic expression handling
    [Fact]
    public void IsArithmeticExpression_ValidExpression_ReturnsTrue()
    {
        CommandParser parser = new CommandParser();
        string arithmeticExpression = "x = y + 5";

        Assert.True(parser.IsVariableDeclarationOrArithmetic(arithmeticExpression));
    }

    // Testing variable replacement in commands
    [Fact]
    public void ReplaceVariables_WithVariables_ReplacesCorrectly()
    {
        CommandParser parser = new CommandParser();
        CommandList commandList = new CommandList(null);
        commandList.SetVariable("x", 20);
        string commandWithVariable = "moveto x 30";

        string replacedCommand = parser.ReplaceVariables(commandWithVariable, commandList);

        Assert.Equal("moveto 20 30", replacedCommand);
    }
}
