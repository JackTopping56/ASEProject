using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace ASEProject
{
    /// <summary>
    /// Represents the main form of the drawing application.
    /// </summary>

    public class CustomInvalidCommandException : Exception
    {
        public CustomInvalidCommandException() { }
        public CustomInvalidCommandException(string message) : base(message) { }
        public CustomInvalidCommandException(string message, Exception inner) : base(message, inner) { }
    }

    public partial class Form1 : Form
    {
        private CommandList commandList;
        private CommandParser commandParser;
        private Dictionary<string, ICommand> commandDictionary;
        private List<string> currentMethodDefinition;
        private bool isDefiningMethod = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            commandList = new CommandList(GraphicsBox.CreateGraphics());
            commandParser = new CommandParser();
            InitializeCommandDictionary();
            CommandBox.KeyPress += CommandBox_KeyPress;
            Open.Click += Open_Click;
            Save.Click += Save_Click;
        }

        /// <summary>
        /// Initializes the command dictionary with command keywords and their corresponding command objects.
        /// </summary>
        private void InitializeCommandDictionary()
        {
            commandDictionary = new Dictionary<string, ICommand>
            {
                {"moveto", new MoveToCommand()},
                {"drawto", new DrawToCommand()},
                {"clear", new ClearCommand()},
                {"reset", new ResetCommand()},
                {"rectangle", new DrawRectangleCommand()},
                {"circle", new DrawCircleCommand()},
                {"triangle", new DrawTriangleCommand()},
                {"pen", new ChangePenColorCommand()},
                {"fill", new ChangeFillModeCommand()}
            };
        }

        /// <summary>
        /// Handles the KeyPress event for the CommandBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A KeyPressEventArgs that contains the event data.</param>
        private void CommandBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                try
                {
                    ExecuteCommandFromTextBox(CommandBox.Text);
                    CommandBox.Clear();
                }
                catch (CustomInvalidCommandException ex)
                {
                    MessageBox.Show($"Invalid Command: {ex.Message}", "Invalid Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private Stack<bool> conditionalStack = new Stack<bool>();
        private bool isInsideLoop = false;
        private List<string> loopCommands = new List<string>();
        private int loopCounter = 0;

        /// <summary>
        /// Executes a command from the command text box.
        /// </summary>
        /// <param name="commandText">The command text to execute.</param>
        private void ExecuteCommandFromTextBox(string commandText)
        {
            string[] commands = commandText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string command in commands)
            {
                string trimmedCommand = command.Trim();

                try
                {
                    if (commandParser.IsMethodDefinition(trimmedCommand))
                    {
                        isDefiningMethod = true;
                        currentMethodDefinition = new List<string> { trimmedCommand };
                    }
                    else if (trimmedCommand.Equals("endmethod", StringComparison.OrdinalIgnoreCase))
                    {
                        if (isDefiningMethod)
                        {
                            currentMethodDefinition.Add(trimmedCommand);
                            var method = commandParser.ParseMethodDefinition(currentMethodDefinition);
                            commandList.AddMethod(method);
                            isDefiningMethod = false;
                            currentMethodDefinition = null;
                        }
                        else
                        {
                            throw new CustomInvalidCommandException("Unexpected 'endmethod'");
                        }
                    }
                    else if (isDefiningMethod)
                    {
                        currentMethodDefinition.Add(trimmedCommand);
                    }
                    else if (commandParser.IsMethodCall(trimmedCommand))
                    {
                        ExecuteMethodCall(trimmedCommand);
                    }
                    else
                    {
                        ProcessRegularCommand(trimmedCommand);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error processing command '{trimmedCommand}': {ex.Message}", "Command Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExecuteMethodCall(string command)
        {
            var parts = command.Split(new[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            var methodName = parts[0].Trim();
            var methodParams = parts[1].Split(',').Select(p => p.Trim()).ToList();

            commandList.ExecuteMethod(methodName, methodParams);
        }

        /// <summary>
        /// Processes a loop command.
        /// </summary>
        /// <param name="command">The loop command to process.</param>
        /// <exception cref="CustomInvalidCommandException">Thrown when nested loops are detected or when loop syntax is invalid.</exception>
        private void ProcessLoopCommand(string command)
        {
            string[] parts = command.Split(' ');
            if (parts[0].ToLower() == "loop")
            {
                if (isInsideLoop)
                {
                    throw new CustomInvalidCommandException("Nested loops are not supported.");
                }

                if (parts.Length != 2 || !int.TryParse(parts[1], out loopCounter))
                {
                    throw new CustomInvalidCommandException("Invalid loop syntax. Correct syntax: loop [number_of_iterations]");
                }
                isInsideLoop = true;
                loopCommands.Clear();
            }
            else if (parts[0].ToLower() == "endloop")
            {
                if (!isInsideLoop)
                {
                    throw new CustomInvalidCommandException("Mismatched 'endloop'");
                }

                for (int i = 0; i < loopCounter; i++)
                {
                    foreach (var loopCommand in loopCommands)
                    {
                        if (commandParser.IsConditionalCommand(loopCommand))
                        {
                            ProcessConditionalCommand(loopCommand);
                        }
                        else
                        {
                            ProcessRegularCommand(loopCommand);
                        }
                    }
                }

                isInsideLoop = false;
            }
        }

        /// <summary>
        /// Determines if the current command should be executed based on conditional logic.
        /// </summary>
        /// <returns>True if the command should be executed; otherwise, false.</returns>
        private bool ShouldExecuteCommand()
        {
            return conditionalStack.Count == 0 || conditionalStack.Peek();
        }

        /// <summary>
        /// Processes a conditional command.
        /// </summary>
        /// <param name="command">The conditional command to process.</param>
        /// <exception cref="CustomInvalidCommandException">Thrown when there is a mismatch in conditional command syntax.</exception>
        private void ProcessConditionalCommand(string command)
        {
            string[] parts = command.Split(' ');
            if (parts[0].ToLower() == "if")
            {
                bool conditionResult = EvaluateCondition(parts[1], parts[2], parts[3]);
                conditionalStack.Push(conditionResult);
            }
            else if (parts[0].ToLower() == "endif")
            {
                if (conditionalStack.Count > 0)
                {
                    conditionalStack.Pop();
                }
                else
                {
                    throw new CustomInvalidCommandException("Mismatched 'endif'");
                }
            }
        }

        /// <summary>
        /// Processes a regular command.
        /// </summary>
        /// <param name="command">The command to process.</param>
        /// <exception cref="CustomInvalidCommandException">Thrown when an unknown command is encountered or when there is an error in arithmetic expression.</exception>
        private void ProcessRegularCommand(string command)
        {
            if (commandParser.IsVariableDeclarationOrArithmetic(command))
            {
                var parts = command.Split('=');
                string variableName = parts[0].Trim();
                int value = EvaluateArithmeticExpression(parts[1].Trim());
                commandList.SetVariable(variableName, value);
            }
            else
            {
                string[] parts = commandParser.ReplaceVariables(command, commandList).Split(' ');
                string commandName = parts[0].ToLower();

                if (commandDictionary.ContainsKey(commandName))
                {
                    commandDictionary[commandName].Execute(commandList, parts);
                }
                else
                {
                    throw new CustomInvalidCommandException($"Unknown command: {command}");
                }
            }
        }

        /// <summary>
        /// Evaluates a condition for a conditional command.
        /// </summary>
        /// <param name="variable">The variable to evaluate.</param>
        /// <param name="operation">The operation for the condition.</param>
        /// <param name="value">The value to compare the variable against.</param>
        /// <returns>True if the condition is met; otherwise, false.</returns>
        /// <exception cref="CustomInvalidCommandException">Thrown when an invalid operation is provided.</exception>
        private bool EvaluateCondition(string variable, string operation, string value)
        {
            int variableValue = commandList.GetVariable(variable);
            int intValue = int.Parse(value);

            switch (operation)
            {
                case ">": return variableValue > intValue;
                case "<": return variableValue < intValue;
                case "==": return variableValue == intValue;
                default: throw new CustomInvalidCommandException($"Invalid operation: {operation}");
            }
        }

        /// <summary>
        /// Evaluates an arithmetic expression.
        /// </summary>
        /// <param name="expression">The arithmetic expression to evaluate.</param>
        /// <returns>The result of the arithmetic expression.</returns>
        /// <exception cref="CustomInvalidCommandException">Thrown when an invalid term is encountered in the arithmetic expression.</exception>
        private int EvaluateArithmeticExpression(string expression)
        {
            // Split the expression by operators
            // Note: This simplistic split by '+' only supports addition and does not respect operator precedence.
            // For a more comprehensive solution, consider implementing or using an existing expression evaluator.
            string[] terms = expression.Split('+');
            int sum = 0;

            foreach (string term in terms)
            {
                string trimmedTerm = term.Trim();
                if (int.TryParse(trimmedTerm, out int number))
                {
                    sum += number;
                }
                else if (commandList.IsVariable(trimmedTerm))
                {
                    sum += commandList.GetVariable(trimmedTerm);
                }
                else
                {
                    throw new CustomInvalidCommandException($"Invalid term in arithmetic expression: {trimmedTerm}");
                }
            }

            return sum;
        }

        /// <summary>
        /// Handles the Click event for the Run button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void btnRun_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Run button clicked");
            string programCode = ProgramCode.Text;
            string[] lines = programCode.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                try
                {
                    ExecuteCommandFromTextBox(line);
                }
                catch (CustomInvalidCommandException ex)
                {
                    MessageBox.Show($"Invalid Command: {ex.Message}", "Invalid Command", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Handles the Click event for the Open menu item.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void Open_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string fileName = openFileDialog.FileName;
                        string fileContent = File.ReadAllText(fileName);
                        ProgramCode.Text = fileContent;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error opening the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event for the Save menu item.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void Save_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string fileName = saveFileDialog.FileName;
                        File.WriteAllText(fileName, ProgramCode.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
