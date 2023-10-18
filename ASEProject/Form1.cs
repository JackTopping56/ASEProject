using System;
using System.Windows.Forms;
using System.IO;

namespace ASEProject
{
    public partial class Form1 : Form
    {
        private CommandList commandList;
        private CommandParser commandParser;

        public Form1()
        {
            InitializeComponent();
            commandList = new CommandList(GraphicsBox.CreateGraphics());
            commandParser = new CommandParser();

            CommandBox.KeyPress += CommandBox_KeyPress;
        }

        private void CommandBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                ExecuteCommandFromTextBox(CommandBox.Text);
                CommandBox.Clear();
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
                    if (string.Equals(trimmedCommand, "clear", StringComparison.OrdinalIgnoreCase))
                    {
                        commandList.Clear();
                        Console.WriteLine("Clear command executed.");
                    }
                    else if (string.Equals(trimmedCommand, "reset", StringComparison.OrdinalIgnoreCase))
                    {
                        commandList.Reset();
                        Console.WriteLine("Reset command executed.");
                    }
                    else if (trimmedCommand.StartsWith("triangle", StringComparison.OrdinalIgnoreCase))
                    {
                        commandList.DrawTriangle(trimmedCommand.Split(' '));
                        Console.WriteLine("Triangle command executed.");
                    }
                    else
                    {
                        commandList.ExecuteCommand(trimmedCommand);
                    }
                }
                else
                {
                    Console.WriteLine($"Invalid command: {trimmedCommand}");
                }
            }
            CommandBox.Clear();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Run button clicked");
            string programCode = ProgramCode.Text;
            string[] lines = programCode.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                ExecuteCommandFromTextBox(line);
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
