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

        // Inside Form1 class

        private Stack<bool> conditionalStack = new Stack<bool>();
        private bool isInsideLoop = false;
        private List<string> loopCommands = new List<string>();
        private int loopCounter = 0;


        private void ExecuteCommandFromTextBox(string commandText)
        {
            string[] commands = commandText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string command in commands)
            {
                string trimmedCommand = command.Trim();

                try
                {
                    if (commandParser.IsLoopCommand(trimmedCommand))
                    {
                        ProcessLoopCommand(trimmedCommand);
                    }
                    else if (isInsideLoop)
                    {
                        // Collect loop commands, including if statements
                        loopCommands.Add(trimmedCommand);
                    }
                    else if (commandParser.IsConditionalCommand(trimmedCommand))
                    {
                        ProcessConditionalCommand(trimmedCommand);
                    }
                    else if (ShouldExecuteCommand())
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
        private bool ShouldExecuteCommand()
        {
            return conditionalStack.Count == 0 || conditionalStack.Peek();
        }

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
