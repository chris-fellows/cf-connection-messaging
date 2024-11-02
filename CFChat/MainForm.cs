using CFChat.Controls;
using CFChat.Models;
using CFConnectionMessaging.Models;

namespace CFChat
{
    public partial class MainForm : Form
    {
        const int receivePort = 11000;
        private ChatConnection _chatConnection;

        public MainForm()
        {
            InitializeComponent();
            
            // Initialise chat connection
            _chatConnection = new ChatConnection(receivePort);            
            _chatConnection.OnChatMessageReceived += _chatConnection_OnChatMessageReceived;
            _chatConnection.OnChatFileReceived += _chatConnection_OnChatFileReceived;

            // Start listening for data
            _chatConnection.StartListening();
        }

        private void _chatConnection_OnChatFileReceived(ChatFile chatFile, MessageReceivedInfo messageReceivedInfo)
        {
            // Get conversation
            var conversation = _chatConnection.Conversations.FirstOrDefault(c => c.ConversationId == chatFile.ConversationId);

            if (conversation == null)       // New conversation
            {
                var conversationControl = new ConversationControl() 
                {  
                    ConversationId = chatFile.ConversationId,
                    RemoteEndpointInfo = new EndpointInfo()
                    {
                        Ip = messageReceivedInfo.RemoteEndpointInfo.Ip,
                        Port = messageReceivedInfo.RemoteEndpointInfo.Port,
                    }
                };
                _chatConnection.Conversations.Add(conversationControl);
                conversation = conversationControl;
            }

            conversation.OnChatFileReceived(chatFile, messageReceivedInfo);
        }

        private void _chatConnection_OnChatMessageReceived(ChatMessage chatMessage, MessageReceivedInfo messageReceivedInfo)
        {
            // Get conversation
            var conversation = _chatConnection.Conversations.FirstOrDefault(c => c.ConversationId == chatMessage.ConversationId);

            if (conversation == null)       // New conversation
            {
                var conversationControl = new ConversationControl()
                {
                    ConversationId = chatMessage.ConversationId,
                    RemoteEndpointInfo = new EndpointInfo()
                    {
                        Ip = messageReceivedInfo.RemoteEndpointInfo.Ip,
                        Port = messageReceivedInfo.RemoteEndpointInfo.Port,
                    }
                };
                _chatConnection.Conversations.Add(conversationControl);
                conversation = conversationControl;
            }

            conversation.OnChatMessageReceived(chatMessage, messageReceivedInfo);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
