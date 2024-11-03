namespace CFChat
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            toolStrip1 = new ToolStrip();
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            startListeningToolStripMenuItem = new ToolStripMenuItem();
            tstxtUserName = new ToolStripTextBox();
            toolStripLabel2 = new ToolStripLabel();
            tstxtLocalPort = new ToolStripTextBox();
            toolStripLabel1 = new ToolStripLabel();
            toolStripLabel3 = new ToolStripLabel();
            tscbConversation = new ToolStripComboBox();
            pnlConversation = new Panel();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton1, tstxtUserName, toolStripLabel2, tstxtLocalPort, toolStripLabel1, toolStripLabel3, tscbConversation });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1074, 25);
            toolStrip1.TabIndex = 5;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { startListeningToolStripMenuItem });
            toolStripDropDownButton1.Image = (Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new Size(54, 22);
            toolStripDropDownButton1.Text = "File";
            // 
            // startListeningToolStripMenuItem
            // 
            startListeningToolStripMenuItem.Name = "startListeningToolStripMenuItem";
            startListeningToolStripMenuItem.Size = new Size(146, 22);
            startListeningToolStripMenuItem.Text = "Start listening";
            startListeningToolStripMenuItem.Click += startListeningToolStripMenuItem_Click;
            // 
            // tstxtUserName
            // 
            tstxtUserName.Alignment = ToolStripItemAlignment.Right;
            tstxtUserName.Name = "tstxtUserName";
            tstxtUserName.Size = new Size(100, 25);
            tstxtUserName.TextChanged += tstxtUserName_TextChanged;
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Alignment = ToolStripItemAlignment.Right;
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(68, 22);
            toolStripLabel2.Text = "User Name:";
            // 
            // tstxtLocalPort
            // 
            tstxtLocalPort.Alignment = ToolStripItemAlignment.Right;
            tstxtLocalPort.Name = "tstxtLocalPort";
            tstxtLocalPort.Size = new Size(100, 25);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Alignment = ToolStripItemAlignment.Right;
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(63, 22);
            toolStripLabel1.Text = "Local Port:";
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new Size(80, 22);
            toolStripLabel3.Text = "Conversation:";
            // 
            // tscbConversation
            // 
            tscbConversation.DropDownStyle = ComboBoxStyle.DropDownList;
            tscbConversation.Name = "tscbConversation";
            tscbConversation.Size = new Size(150, 25);
            tscbConversation.SelectedIndexChanged += tscbConversation_SelectedIndexChanged;
            // 
            // pnlConversation
            // 
            pnlConversation.Dock = DockStyle.Fill;
            pnlConversation.Location = new Point(0, 25);
            pnlConversation.Name = "pnlConversation";
            pnlConversation.Size = new Size(1074, 631);
            pnlConversation.TabIndex = 6;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1074, 656);
            Controls.Add(pnlConversation);
            Controls.Add(toolStrip1);
            Name = "MainForm";
            Text = "Form1";
            FormClosing += MainForm_FormClosing;
            Load += Form1_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem startListeningToolStripMenuItem;
        private ToolStripLabel toolStripLabel1;
        private ToolStripTextBox tstxtLocalPort;
        private ToolStripLabel toolStripLabel2;
        private ToolStripTextBox tstxtUserName;
        private ToolStripLabel toolStripLabel3;
        private ToolStripComboBox tscbConversation;
        private Panel pnlConversation;
    }
}
