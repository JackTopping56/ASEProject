namespace ASEProject
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Open = new Button();
            Save = new Button();
            btnRun = new Button();
            btnSyntax = new Button();
            ProgramCode = new TextBox();
            GraphicsBox = new PictureBox();
            CommandBox = new TextBox();
            ((System.ComponentModel.ISupportInitialize)GraphicsBox).BeginInit();
            SuspendLayout();
            // 
            // Open
            // 
            Open.Location = new Point(12, 24);
            Open.Name = "Open";
            Open.Size = new Size(75, 23);
            Open.TabIndex = 0;
            Open.Text = "Open";
            Open.UseVisualStyleBackColor = true;
            Open.Click += Open_Click;
            // 
            // Save
            // 
            Save.Location = new Point(117, 24);
            Save.Name = "Save";
            Save.Size = new Size(75, 23);
            Save.TabIndex = 1;
            Save.Text = "Save";
            Save.UseVisualStyleBackColor = true;
            Save.Click += Save_Click;
            // 
            // btnRun
            // 
            btnRun.Location = new Point(410, 548);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(75, 23);
            btnRun.TabIndex = 2;
            btnRun.Text = "Run";
            btnRun.UseVisualStyleBackColor = true;
            // 
            // btnSyntax
            // 
            btnSyntax.Location = new Point(561, 548);
            btnSyntax.Name = "btnSyntax";
            btnSyntax.Size = new Size(75, 23);
            btnSyntax.TabIndex = 3;
            btnSyntax.Text = "Syntax";
            btnSyntax.UseVisualStyleBackColor = true;
            // 
            // ProgramCode
            // 
            ProgramCode.Location = new Point(12, 65);
            ProgramCode.Multiline = true;
            ProgramCode.Name = "ProgramCode";
            ProgramCode.Size = new Size(473, 421);
            ProgramCode.TabIndex = 4;
            // 
            // GraphicsBox
            // 
            GraphicsBox.BackColor = SystemColors.ControlDark;
            GraphicsBox.Location = new Point(561, 65);
            GraphicsBox.Name = "GraphicsBox";
            GraphicsBox.Size = new Size(473, 421);
            GraphicsBox.TabIndex = 5;
            GraphicsBox.TabStop = false;
            // 
            // CommandBox
            // 
            CommandBox.Location = new Point(322, 507);
            CommandBox.Name = "CommandBox";
            CommandBox.Size = new Size(400, 23);
            CommandBox.TabIndex = 6;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1128, 651);
            Controls.Add(CommandBox);
            Controls.Add(GraphicsBox);
            Controls.Add(ProgramCode);
            Controls.Add(btnSyntax);
            Controls.Add(btnRun);
            Controls.Add(Save);
            Controls.Add(Open);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)GraphicsBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Open;
        private Button Save;
        private Button btnRun;
        private Button btnSyntax;
        private TextBox ProgramCode;
        private PictureBox GraphicsBox;
        private TextBox CommandBox;
    }
}