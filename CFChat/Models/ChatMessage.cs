namespace CFChat.Models
{
    /// <summary>
    /// Chat message
    /// </summary>
    public class ChatMessage : ChatBase
    {
        public string Text { get; set; } = String.Empty;
    }
}
