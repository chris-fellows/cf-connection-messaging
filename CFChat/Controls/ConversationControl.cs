using CFChat.Interfaces;
using CFChat.Models;
using CFConnectionMessaging.Models;

namespace CFChat.Controls
{
    /// <summary>
    /// Conversation control which implements IConversation. Handles single conversation. All messages have 
    /// same ConversationId.
    /// 
    /// ConversationId is either set if sending the first message or if receiving a message and it's not
    /// set.
    /// </summary>
    public partial class ConversationControl : UserControl, IConversation
    {        
        public ConversationControl()
        {
            InitializeComponent();
        }
       
        public ChatConnection ChatConnection { get; set; }

        public string ConversationId { get; set; }

        public EndpointInfo RemoteEndpointInfo { get; set; } = new EndpointInfo();

        /// <summary>
        /// Adds a message to the Messages list
        /// </summary>
        /// <param name="message"></param>
        private void AddMessageLine(string message)
        {
            lstMessage.Items.Add(message);

            // Limit messages received
            while (lstMessage.Items.Count > 500)
            {
                lstMessage.Items.RemoveAt(0);
            }
        }

        private string ChatFileReceivedFolder
        {
            get { return Path.Combine(Environment.CurrentDirectory, ConversationId); }
        }

        public void OnChatMessageReceived(ChatMessage chatMessage, MessageReceivedInfo messageReceivedInfo)
        {
            this.Invoke((Action)delegate    // UI thread
            {
                SetRemoteEndpointIfNotSet(messageReceivedInfo);

                if (lstMessage.Items.Count > 0) AddMessageLine("");
                AddMessageLine($"Message received from {chatMessage.SenderName}: {messageReceivedInfo.ReceivedTime.ToString()}");
                AddMessageLine(chatMessage.Text);
            });
        }

        /// <summary>
        /// Sets remote endpoint info from received message if not set
        /// </summary>
        /// <param name="messageReceivedInfo"></param>
        private void SetRemoteEndpointIfNotSet(MessageReceivedInfo messageReceivedInfo)
        {
            if (String.IsNullOrEmpty(RemoteEndpointInfo.Ip))
            {
                RemoteEndpointInfo.Ip = messageReceivedInfo.RemoteEndpointInfo.Ip;
                RemoteEndpointInfo.Port = messageReceivedInfo.RemoteEndpointInfo.Port;

                tstxtRemoteIP.Text = RemoteEndpointInfo.Ip;
                tstxtRemotePort.Text = RemoteEndpointInfo.Port.ToString();
            }
        }

        public void OnChatFileReceived(ChatFile chatFile, MessageReceivedInfo messageReceivedInfo)
        {
            this.Invoke((Action)delegate    // UI thread
            {
                SetRemoteEndpointIfNotSet(messageReceivedInfo);

                var localFile = SaveReceivedFile(chatFile);

                if (lstMessage.Items.Count > 0) AddMessageLine("");
                AddMessageLine($"File received from {chatFile.SenderName}: {messageReceivedInfo.ReceivedTime.ToString()}");
                AddMessageLine($"{chatFile.Name}: Saved to {localFile}");
            });
        }

        public void OnPingRequestReceived(PingRequest pingRequest, MessageReceivedInfo messageReceivedInfo)
        {
            this.Invoke((Action)delegate    // UI thread
            {
                SetRemoteEndpointIfNotSet(messageReceivedInfo);

                if (lstMessage.Items.Count > 0) AddMessageLine("");
                AddMessageLine($"Ping received from {pingRequest.SenderName}: {messageReceivedInfo.ReceivedTime.ToString()}");

                // Send response
                var pingResponse = new PingResponse()
                {
                    Id = Guid.NewGuid().ToString(),
                    ConversationId = pingRequest.ConversationId,
                    SenderName = ChatConnection.LocalUserName
                };
                ChatConnection.SendPingResponse(pingResponse, RemoteEndpointInfo);
            });
        }

        public void OnPingResponseReceived(PingResponse pingResponse, MessageReceivedInfo messageReceivedInfo)
        {
            this.Invoke((Action)delegate    // UI thread
            {
                SetRemoteEndpointIfNotSet(messageReceivedInfo);

                if (lstMessage.Items.Count > 0) AddMessageLine("");
                AddMessageLine($"Ping response from {pingResponse.SenderName}: {messageReceivedInfo.ReceivedTime.ToString()}");             
            });
        }

        private string SaveReceivedFile(ChatFile chatFile)
        {            
            var localFile = Path.Combine(ChatFileReceivedFolder, chatFile.Name);
            Directory.CreateDirectory(ChatFileReceivedFolder);
            if (File.Exists(localFile))
            {
                File.Delete(localFile);
            }
            File.WriteAllBytes(localFile, chatFile.Content);

            return localFile;
        }

        private void tsbSend_Click(object sender, EventArgs e)
        {
            SetRemoteEndpointIfNotSetFromUI();

            if (String.IsNullOrEmpty(RemoteEndpointInfo.Ip))
            {
                MessageBox.Show("Please set remote endpoint information", "Error");
                return;
            }

            var sentTime = DateTimeOffset.UtcNow;

            // Set ConversationId if not set
            if (String.IsNullOrEmpty(ConversationId))
            {
                ConversationId = Guid.NewGuid().ToString();
            }

            var chatMessage = new ChatMessage()
            {
                Id = Guid.NewGuid().ToString(),
                ConversationId = ConversationId,
                SenderName = ChatConnection.LocalUserName,
                Text = txtMessage.Text
            };
            ChatConnection.SendChatMessage(chatMessage, RemoteEndpointInfo);

            // Log sent
            if (lstMessage.Items.Count > 0) AddMessageLine("");
            AddMessageLine($"Message sent: {sentTime.ToString()}");
            AddMessageLine(chatMessage.Text);

            // Clear message
            txtMessage.Text = "";
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            tsbSendMessage.Enabled = txtMessage.Text.Trim().Length > 0;
        }

        private void SetRemoteEndpointIfNotSetFromUI()
        {
            // Set RemoteEndpointInfo from UI
            if (String.IsNullOrEmpty(RemoteEndpointInfo.Ip))
            {
                RemoteEndpointInfo.Ip = tstxtRemoteIP.Text;
                RemoteEndpointInfo.Port = Convert.ToInt32("0" + tstxtRemotePort.Text);
            }
        }

        private void tsbSendFile_Click(object sender, EventArgs e)
        {
            SetRemoteEndpointIfNotSetFromUI();

            if (String.IsNullOrEmpty(RemoteEndpointInfo.Ip))
            {
                MessageBox.Show("Please set remote endpoint information", "Error");
                return;
            }

            var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                Title = "Select file"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Set ConversationId if not set
                if (String.IsNullOrEmpty(ConversationId))
                {
                    ConversationId = Guid.NewGuid().ToString();
                }

                var sentTime = DateTimeOffset.UtcNow;

                var chatFile = new ChatFile()
                {
                    Id = Guid.NewGuid().ToString(),
                    ConversationId = ConversationId,
                    SenderName = ChatConnection.LocalUserName,
                    Content = File.ReadAllBytes(dialog.FileName),
                    Name = Path.GetFileName(dialog.FileName)
                };
                ChatConnection.SendChatFile(chatFile, RemoteEndpointInfo);

                // Lot sent
                if (lstMessage.Items.Count > 0) AddMessageLine("");
                AddMessageLine($"File sent: {sentTime.ToString()}");
                AddMessageLine(chatFile.Name);
            }
        }

        private void tsbOpenReceivedFilesFolder_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(ChatFileReceivedFolder))
            {
                using (System.Diagnostics.Process process = new System.Diagnostics.Process())
                {
                    process.StartInfo.FileName = "Explorer.exe";
                    process.StartInfo.Arguments = ChatFileReceivedFolder;
                    process.Start();
                }
            }
        }
    }
}
