using CFChat.Models;
using CFConnectionMessaging.Models;

namespace CFChat.Interfaces
{
    /// <summary>
    /// Interface to conversation
    /// </summary>
    public interface IConversation
    {
        string ConversationId { get; }

        EndpointInfo RemoteEndpointInfo { get; }

        /// <summary>
        /// Handles chat message received from endpoint
        /// </summary>
        /// <param name="chatMessage"></param>
        /// <param name="messageReceivedInfo"></param>
        void OnChatMessageReceived(ChatMessage chatMessage, MessageReceivedInfo messageReceivedInfo);

        /// <summary>
        /// Handles chat file received from endpoint
        /// </summary>
        /// <param name="chatFile"></param>
        /// <param name="messageReceivedInfo"></param>
        void OnChatFileReceived(ChatFile chatFile, MessageReceivedInfo messageReceivedInfo);

        /// <summary>
        /// Handles ping request received from endpoint
        /// </summary>
        /// <param name="pingRequest"></param>
        /// <param name="messageReceivedInfo"></param>
        void OnPingRequestReceived(PingRequest pingRequest, MessageReceivedInfo messageReceivedInfo);

        /// <summary>
        /// Handles ping response received from endpoint
        /// </summary>
        /// <param name="pingResponse"></param>
        /// <param name="messageReceivedInfo"></param>
        void OnPingResponseReceived(PingResponse pingResponse, MessageReceivedInfo messageReceivedInfo);
    }
}
