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
            tsbSend = new ToolStripButton();
            splitContainer1 = new SplitContainer();
            txtMessage = new TextBox();
            lstMessage = new ListView();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { tsbSend });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1023, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // tsbSend
            // 
            tsbSend.Image = (Image)resources.GetObject("tsbSend.Image");
            tsbSend.ImageTransparentColor = Color.Magenta;
            tsbSend.Name = "tsbSend";
            tsbSend.Size = new Size(53, 22);
            tsbSend.Text = "Send";
            tsbSend.Click += tsbSend_Click;
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
            splitContainer1.SplitterDistance = 341;
            splitContainer1.TabIndex = 1;
            // 
            // txtMessage
            // 
            txtMessage.Dock = DockStyle.Fill;
            txtMessage.Location = new Point(0, 0);
            txtMessage.Multiline = true;
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(1023, 276);
            txtMessage.TabIndex = 0;
            txtMessage.TextChanged += txtMessage_TextChanged;
            // 
            // lstMessage
            // 
            lstMessage.Dock = DockStyle.Fill;
            lstMessage.Location = new Point(0, 0);
            lstMessage.Name = "lstMessage";
            lstMessage.Size = new Size(1023, 341);
            lstMessage.TabIndex = 0;
            lstMessage.UseCompatibleStateImageBehavior = false;
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
        private ToolStripButton tsbSend;
        private ListView lstMessage;
    }
}
