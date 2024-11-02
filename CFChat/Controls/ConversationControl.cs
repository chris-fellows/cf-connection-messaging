using CFChat.Interfaces;
using CFChat.Models;
using CFConnectionMessaging.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFChat.Controls
{
    /// <summary>
    /// Conversation control.
    /// </summary>
    public partial class ConversationControl : UserControl, IConversation
    {
        //private const int _maxChatMessages = 500;
        private readonly ChatConnection _chatConnection;

        //private List<ChatMessage> _chatMessages = new List<ChatMessage>();

        public ConversationControl()
        {
            InitializeComponent();
        }

        public ConversationControl(ChatConnection chatConnection)
        {
            InitializeComponent();

            _chatConnection = chatConnection;
        }

        public string ConversationId { get; set; }

        public EndpointInfo RemoteEndpointInfo { get; set; } = new EndpointInfo();

        public void OnChatMessageReceived(ChatMessage chatMessage, MessageReceivedInfo messageReceivedInfo)
        {           
            var received = DateTimeOffset.UtcNow;
            lstMessage.Items.Add($"Message received: {messageReceivedInfo.ReceivedTime.ToString()}");
            lstMessage.Items.Add(chatMessage.Text);

            // Limit messages received
            while (lstMessage.Items.Count > 500)
            {
                lstMessage.Items.RemoveAt(0);
            }
        }

        public void OnChatFileReceived(ChatFile chatFile, MessageReceivedInfo messageReceivedInfo)
        {            
            lstMessage.Items.Add($"File received: {messageReceivedInfo.ReceivedTime.ToString()}");
            lstMessage.Items.Add(chatFile.Name);

            // Limit messages received
            while (lstMessage.Items.Count > 500)
            {
                lstMessage.Items.RemoveAt(0);
            }
        }

        //public void SendChatMessage(ChatMessage chatMessage)
        //{
        //    _chatConnection.SendChatMessage(chatMessage);
        //}

        //public void SendChatFile(ChatFile chatFile)
        //{
        //    _chatConnection.SendChatFile(chatFile);
        //}

        private void tsbSend_Click(object sender, EventArgs e)
        {
            var chatMessage = new ChatMessage()
            {
                ConversationId = ConversationId,
                SenderName = "X",
                Text = txtMessage.Text
            };
            _chatConnection.SendChatMessage(chatMessage, RemoteEndpointInfo);
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            tsbSend.Enabled = txtMessage.Text.Trim().Length > 0;
        }
    }
}
