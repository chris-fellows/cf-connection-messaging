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
    /// Chat connection. Sends and receives chat messages via TCP connection.
    /// 
    /// The connection supports multiple conversations. Each message specifies a ConversationId so that the message
    /// is routed to the correct IConversation.
    /// </summary>
    public class ChatConnection
    {
        //private ConnectionUdp _connection;
        private ConnectionTcp _connection;

        private List<IConversation> _conversations = new List<IConversation>();

        private IExternalMessageConverter<ChatFile> _chatFileMessageConverter = new ChatFileMessageConverter();
        private IExternalMessageConverter<ChatMessage> _chatMessageMessageConverter = new ChatMessageMessageConverter();
        private IExternalMessageConverter<PingRequest> _pingRequestMessageConverter = new PingRequestMessageConverter();
        private IExternalMessageConverter<PingResponse> _pingResponseMessageConverter = new PingResponseMessageConverter();

        public string LocalUserName { get; set; } = String.Empty;

        public ChatConnection()
        {            
            _connection = new ConnectionTcp();            
            _connection.OnConnectionMessageReceived += _connection_OnConnectionMessageReceived;
        }

        public List<IConversation> Conversations
        {
            get { return _conversations; }
        }

        public void StartListening(int port)
        {
            _connection.ReceivePort = port;
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

        /// <summary>
        /// Send ping request
        /// </summary>
        /// <param name="pingRequest"></param>
        /// <param name="remoteEndpointInfo"></param>
        public void SendPingRequest(PingRequest pingRequest, EndpointInfo remoteEndpointInfo)
        {
            _connection.SendMessage(_pingRequestMessageConverter.GetConnectionMessage(pingRequest), remoteEndpointInfo);
        }

        /// <summary>
        /// Send ping response
        /// </summary>
        /// <param name="pingRequest"></param>
        /// <param name="remoteEndpointInfo"></param>
        public void SendPingResponse(PingResponse pingResponse, EndpointInfo remoteEndpointInfo)
        {
            _connection.SendMessage(_pingResponseMessageConverter.GetConnectionMessage(pingResponse), remoteEndpointInfo);
        }

        /// <summary>
        /// Handles ConnectionMessage received. Determines message type, converts to chat message, forwards it to the
        /// correct IConversation instance.
        ///
        /// We assume that the UI has populated Conversations with a free IConversation that can be used if a message is received
        /// for a new conversation.
        /// </summary>
        /// <param name="connectionMessage"></param>
        /// <param name="messageReceivedInfo"></param>
        private void _connection_OnConnectionMessageReceived(ConnectionMessage connectionMessage, MessageReceivedInfo messageReceivedInfo)
        {
            switch(connectionMessage.TypeId)
            {
                case MessageTypeIds.ChatMessage:                    
                    {
                        // Get ChatMessage
                        var chatMessage = _chatMessageMessageConverter.GetExternalMessage(connectionMessage);

                        // Get conversation
                        var conversation = Conversations.FirstOrDefault(c => c.ConversationId == chatMessage.ConversationId);

                        // If no conversation then see if there's an unused IConversation
                        if (conversation == null)
                        {
                            conversation = Conversations.FirstOrDefault(c => String.IsNullOrEmpty(c.ConversationId));                           
                        }

                        // Pass message to conversation
                        if (conversation != null)
                        {
                            conversation.OnChatMessageReceived(chatMessage, messageReceivedInfo);
                        }
                    }
                    break;
                case MessageTypeIds.ChatFile:                    
                    {
                        // Get ChatFile
                        var chatFile = _chatFileMessageConverter.GetExternalMessage(connectionMessage);

                        // Get conversation
                        var conversation = Conversations.FirstOrDefault(c => c.ConversationId == chatFile.ConversationId);

                        // If no conversation then see if there's an unused IConversation
                        if (conversation == null)
                        {
                            conversation = Conversations.FirstOrDefault(c => String.IsNullOrEmpty(c.ConversationId));
                        }

                        // Pass message to conversation
                        if (conversation != null)
                        {
                            conversation.OnChatFileReceived(chatFile, messageReceivedInfo);
                        }
                    }
                    break;
                case MessageTypeIds.PingRequest:                                    
                    {
                        // Get PingRequest
                        var pingRequest = _pingRequestMessageConverter.GetExternalMessage(connectionMessage);

                        // Get conversation
                        var conversation = Conversations.FirstOrDefault(c => c.ConversationId == pingRequest.ConversationId);

                        // If no conversation then see if there's an unused IConversation
                        if (conversation == null)
                        {
                            conversation = Conversations.FirstOrDefault(c => String.IsNullOrEmpty(c.ConversationId));
                        }

                        // Pass message to conversation
                        if (conversation != null)
                        {
                            conversation.OnPingRequestReceived(pingRequest, messageReceivedInfo);
                        }
                     }
                     break;
                case MessageTypeIds.PingResponse:                    
                    {
                        // Get PingResponse
                        var pingResponse = _pingResponseMessageConverter.GetExternalMessage(connectionMessage);

                        // Get conversation
                        var conversation = Conversations.FirstOrDefault(c => c.ConversationId == pingResponse.ConversationId);

                        // If no conversation then see if there's an unused IConversation
                        if (conversation == null)
                        {
                            conversation = Conversations.FirstOrDefault(c => String.IsNullOrEmpty(c.ConversationId));
                        }

                        // Pass message to conversation
                        if (conversation != null)
                        {
                            conversation.OnPingResponseReceived(pingResponse, messageReceivedInfo);
                        }
                    }
                    break;                          
            }
        }  
    }
}
