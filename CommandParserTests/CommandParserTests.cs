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

    /// <summary>
    /// Tests if the CommandParser correctly identifies a valid conditional command.
    /// </summary>
    [Fact]
    public void IsConditionalCommand_ValidIf_ReturnsTrue()
    {
        CommandParser parser = new CommandParser();
        string ifCommand = "if x > 10";

        Assert.True(parser.IsConditionalCommand(ifCommand));
    }

    /// <summary>
    /// Tests if the CommandParser correctly identifies a valid loop command.
    /// </summary>
    [Fact]
    public void IsLoopCommand_ValidLoop_ReturnsTrue()
    {
        CommandParser parser = new CommandParser();
        string loopCommand = "loop 5";

        Assert.True(parser.IsLoopCommand(loopCommand));
    }

    /// <summary>
    /// Tests if the CommandParser correctly identifies a valid variable declaration.
    /// </summary>
    [Fact]
    public void IsVariableDeclaration_ValidDeclaration_ReturnsTrue()
    {
        CommandParser parser = new CommandParser();
        string varDeclaration = "x = 10";

        Assert.True(parser.IsVariableDeclarationOrArithmetic(varDeclaration));
    }

    /// <summary>
    /// Tests if the CommandParser correctly identifies a valid arithmetic expression.
    /// </summary>
    [Fact]
    public void IsArithmeticExpression_ValidExpression_ReturnsTrue()
    {
        CommandParser parser = new CommandParser();
        string arithmeticExpression = "x = y + 5";

        Assert.True(parser.IsVariableDeclarationOrArithmetic(arithmeticExpression));
    }

    /// <summary>
    /// Tests if the CommandParser correctly replaces variables in a command with their actual values.
    /// </summary>
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

    /// <summary>
    /// Tests if the CommandParser correctly identifies a valid method definition.
    /// </summary>
    [Fact]
    public void IsMethodDefinition_ValidMethodDefinition_ReturnsTrue()
    {
        CommandParser parser = new CommandParser();
        string methodDefinition = "method drawSmallCircle x y";

        Assert.True(parser.IsMethodDefinition(methodDefinition));
    }

    /// <summary>
    /// Tests if the CommandParser correctly identifies a valid method call.
    /// </summary>
    [Fact]
    public void IsMethodCall_ValidMethodCall_ReturnsTrue()
    {
        CommandParser parser = new CommandParser();
        string methodCall = "drawSmallCircle(20, 20)";

        Assert.True(parser.IsMethodCall(methodCall));
    }

    /// <summary>
    /// Tests if the CommandParser correctly parses a complete method definition.
    /// </summary>
    [Fact]
    public void ParseMethodDefinition_ValidDefinition_ParsesCorrectly()
    {
        CommandParser parser = new CommandParser();
        var commandList = new CommandList(null); // Assuming null is an acceptable Graphics object here
        List<string> methodDefinitionLines = new List<string>
        {
            "method drawSmallCircle x y",
            "moveto x y",
            "circle 5",
            "endmethod"
        };

        var method = parser.ParseMethodDefinition(methodDefinitionLines);

        Assert.NotNull(method);
        Assert.Equal("drawSmallCircle", method.Name);
        Assert.Equal(new List<string> { "x", "y" }, method.Parameters);
        Assert.Equal(new List<string> { "moveto x y", "circle 5" }, method.Commands);
    }

    /// <summary>
    /// Tests if the CommandParser correctly validates a method call format to an existing method.
    /// </summary>
    [Fact]
    public void IsValidMethodCall_CorrectFormatToExistingMethod_ReturnsTrue()
    {
        // Arrange
        CommandParser parser = new CommandParser();
        CommandList commandList = new CommandList(null);

        // Add a mock method to the command list
        Method mockMethod = new Method("drawCircle")
        {
            Parameters = new List<string> { "radius" }
        };
        commandList.AddMethod(mockMethod);

        // Act
        bool isValid = parser.IsValidMethodCall("drawCircle(10)", commandList);

        // Assert
        Assert.True(isValid);
    }
}
