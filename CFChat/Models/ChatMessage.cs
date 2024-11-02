namespace CFChat.Models
{
    /// <summary>
    /// Chat message
    /// </summary>
    public class ChatMessage
    {
        public string ConversationId { get; set; } = String.Empty;

        public string SenderName { get; set; } = String.Empty;

        public string Text { get; set; } = String.Empty;
    }
}
