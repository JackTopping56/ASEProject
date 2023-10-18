namespace ASEProject
{
    public partial class Form1 : Form
    {

        private CommandList commandList;
        public Form1()
        {
            InitializeComponent();
            commandList = new CommandList(GraphicsBox.CreateGraphics());
            CommandBox.KeyPress += CommandBox_KeyPress;
        }

        private void CommandBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Prevent the Enter key from being added to the text
                ExecuteCommandFromTextBox(CommandBox.Text);
                CommandBox.Clear(); // Clear the CommandBox after executing commands.
            }
        }

        private void ExecuteCommandFromTextBox(string commandText)
        {
            string[] commands = commandText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string command in commands)
            {
                string trimmedCommand = command.Trim();
                Console.WriteLine($"Executing command: {trimmedCommand}"); // Debug output

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
            CommandBox.Clear();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            ExecuteCommandFromTextBox(ProgramCode.Text);
        }


        private void Open_Click(object sender, EventArgs e)
        {

        }

        private void Save_Click(object sender, EventArgs e)
        {

        }
    }
}