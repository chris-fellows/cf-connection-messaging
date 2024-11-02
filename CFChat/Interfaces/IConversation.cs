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

        void OnChatMessageReceived(ChatMessage chatMessage, MessageReceivedInfo messageReceivedInfo);

        void OnChatFileReceived(ChatFile chatFile, MessageReceivedInfo messageReceivedInfo);
    }
}
