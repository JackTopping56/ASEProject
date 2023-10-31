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

        private void ExecuteCommandFromTextBox(string commandText)
        {
            string[] commands = commandText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string command in commands)
            {
                string trimmedCommand = command.Trim();
                Console.WriteLine($"Executing command: {trimmedCommand}");

                if (commandParser.IsValidCommand(trimmedCommand) && commandParser.HasValidParameters(trimmedCommand))
                {
                    string[] parts = trimmedCommand.Split(' ');
                    string commandName = parts[0].ToLower();

                    if (commandDictionary.ContainsKey(commandName))
                    {
                        commandDictionary[commandName].Execute(commandList, parts);
                        Console.WriteLine($"{trimmedCommand} command executed.");
                    }
                    else
                    {
                        Console.WriteLine($"Unknown command: {trimmedCommand}");
                    }
                }
                else
                {
                    Console.WriteLine($"Invalid command: {trimmedCommand}");
                }
            }
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
