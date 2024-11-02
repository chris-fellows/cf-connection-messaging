using CFChat.Constants;
using CFChat.Interfaces;
using CFChat.MessageConverters;
using CFChat.Models;
using CFConnectionMessaging;
using CFConnectionMessaging.Interfaces;
using CFConnectionMessaging.Models;

namespace CFChat
{
    /// <summary>
    /// Chat connection
    /// </summary>
    public class ChatConnection
    {
        private Connection _connection;
        
        /// <summary>
        /// Event for chat message received
        /// </summary>
        /// <param name="chatMessage"></param>
        public delegate void ChatMessageReceived(ChatMessage chatMessage, MessageReceivedInfo messageReceivedInfo);
        public event ChatMessageReceived? OnChatMessageReceived;

        /// <summary>
        /// Event for chat file received
        /// </summary>
        /// <param name="chatFile"></param>
        public delegate void ChatFileReceived(ChatFile chatFile, MessageReceivedInfo messageReceivedInfo);
        public event ChatFileReceived? OnChatFileReceived;

        private List<IConversation> _conversations = new List<IConversation>();

        private IExternalMessageConverter<ChatFile> _chatFileMessageConverter = new ChatFileMessageConverter();
        private IExternalMessageConverter<ChatMessage> _chatMessageMessageConverter = new ChatMessageMessageConverter();

        public ChatConnection(int receivePort)
        {            
            _connection = new Connection();
            _connection.ReceivePort = receivePort;
            _connection.OnConnectionMessageReceived += _connection_OnConnectionMessageReceived;
        }

        public List<IConversation> Conversations
        {
            get { return _conversations; }
        }

        public void StartListening()
        {
            _connection.StartListening();
        }

        public void StopListening()
        {
            _connection.StopListening();
        }

        /// <summary>
        /// Send chat message
        /// </summary>
        /// <param name="chatMessage"></param>
        public void SendChatMessage(ChatMessage chatMessage, EndpointInfo remoteEndpointInfo)
        {            
            _connection.SendMessage(_chatMessageMessageConverter.GetConnectionMessage(chatMessage), remoteEndpointInfo );
        }        

        /// <summary>
        /// Send chat file
        /// </summary>
        /// <param name="chatFile"></param>
        public void SendChatFile(ChatFile chatFile, EndpointInfo remoteEndpointInfo)
        {
            _connection.SendMessage(_chatFileMessageConverter.GetConnectionMessage(chatFile), remoteEndpointInfo);
        }

        private void _connection_OnConnectionMessageReceived(ConnectionMessage connectionMessage, MessageReceivedInfo messageReceivedInfo)
        {
            switch(connectionMessage.TypeId)
            {
                case MessageTypeIds.ChatMessage:
                    if (OnChatMessageReceived != null)
                    {
                        // Get ChatMessage
                        var chatMessage = _chatMessageMessageConverter.GetExternalMessage(connectionMessage);

                        // Notify
                        OnChatMessageReceived(chatMessage, messageReceivedInfo);
                    }
                    break;
                case MessageTypeIds.ChatFile:
                    if (OnChatFileReceived!= null)
                    {
                        // Get ChatFile
                        var chatFile = _chatFileMessageConverter.GetExternalMessage(connectionMessage);

                        // Notify
                        OnChatFileReceived(chatFile, messageReceivedInfo);
                    }
                    break;
            }
        }  
    }
}
