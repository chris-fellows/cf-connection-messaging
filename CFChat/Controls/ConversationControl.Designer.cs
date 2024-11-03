namespace CFChat.Controls
{
    partial class ConversationControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConversationControl));
            toolStrip1 = new ToolStrip();
            tsbSendMessage = new ToolStripButton();
            tsbSendFile = new ToolStripButton();
            tsbOpenReceivedFilesFolder = new ToolStripButton();
            tstxtRemotePort = new ToolStripTextBox();
            toolStripLabel2 = new ToolStripLabel();
            tstxtRemoteIP = new ToolStripTextBox();
            toolStripLabel1 = new ToolStripLabel();
            splitContainer1 = new SplitContainer();
            lstMessage = new ListView();
            txtMessage = new TextBox();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { tsbSendMessage, tsbSendFile, tsbOpenReceivedFilesFolder, tstxtRemotePort, toolStripLabel2, tstxtRemoteIP, toolStripLabel1 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1023, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // tsbSendMessage
            // 
            tsbSendMessage.Image = (Image)resources.GetObject("tsbSendMessage.Image");
            tsbSendMessage.ImageTransparentColor = Color.Magenta;
            tsbSendMessage.Name = "tsbSendMessage";
            tsbSendMessage.Size = new Size(53, 22);
            tsbSendMessage.Text = "Send";
            tsbSendMessage.Click += tsbSend_Click;
            // 
            // tsbSendFile
            // 
            tsbSendFile.Image = (Image)resources.GetObject("tsbSendFile.Image");
            tsbSendFile.ImageTransparentColor = Color.Magenta;
            tsbSendFile.Name = "tsbSendFile";
            tsbSendFile.Size = new Size(72, 22);
            tsbSendFile.Text = "Send file";
            tsbSendFile.Click += tsbSendFile_Click;
            // 
            // tsbOpenReceivedFilesFolder
            // 
            tsbOpenReceivedFilesFolder.Image = (Image)resources.GetObject("tsbOpenReceivedFilesFolder.Image");
            tsbOpenReceivedFilesFolder.ImageTransparentColor = Color.Magenta;
            tsbOpenReceivedFilesFolder.Name = "tsbOpenReceivedFilesFolder";
            tsbOpenReceivedFilesFolder.Size = new Size(161, 22);
            tsbOpenReceivedFilesFolder.Text = "Open received files folder";
            tsbOpenReceivedFilesFolder.Click += tsbOpenReceivedFilesFolder_Click;
            // 
            // tstxtRemotePort
            // 
            tstxtRemotePort.Alignment = ToolStripItemAlignment.Right;
            tstxtRemotePort.Name = "tstxtRemotePort";
            tstxtRemotePort.Size = new Size(70, 25);
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Alignment = ToolStripItemAlignment.Right;
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(76, 22);
            toolStripLabel2.Text = "Remote Port:";
            // 
            // tstxtRemoteIP
            // 
            tstxtRemoteIP.Alignment = ToolStripItemAlignment.Right;
            tstxtRemoteIP.Name = "tstxtRemoteIP";
            tstxtRemoteIP.Size = new Size(120, 25);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Alignment = ToolStripItemAlignment.Right;
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(64, 22);
            toolStripLabel1.Text = "Remote IP:";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 25);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(lstMessage);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(txtMessage);
            splitContainer1.Size = new Size(1023, 621);
            splitContainer1.SplitterDistance = 501;
            splitContainer1.TabIndex = 1;
            // 
            // lstMessage
            // 
            lstMessage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstMessage.Location = new Point(0, 3);
            lstMessage.Name = "lstMessage";
            lstMessage.Size = new Size(1023, 498);
            lstMessage.TabIndex = 0;
            lstMessage.UseCompatibleStateImageBehavior = false;
            lstMessage.View = View.List;
            // 
            // txtMessage
            // 
            txtMessage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtMessage.Location = new Point(0, 0);
            txtMessage.Multiline = true;
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(1023, 116);
            txtMessage.TabIndex = 0;
            txtMessage.TextChanged += txtMessage_TextChanged;
            // 
            // ConversationControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Controls.Add(toolStrip1);
            Name = "ConversationControl";
            Size = new Size(1023, 646);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip1;
        private SplitContainer splitContainer1;
        private TextBox txtMessage;
        private ToolStripButton tsbSendMessage;
        private ListView lstMessage;
        private ToolStripButton tsbSendFile;
        private ToolStripButton tsbOpenReceivedFilesFolder;
        private ToolStripLabel toolStripLabel1;
        private ToolStripTextBox tstxtRemoteIP;
        private ToolStripLabel toolStripLabel2;
        private ToolStripTextBox tstxtRemotePort;
    }
}
