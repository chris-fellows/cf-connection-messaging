namespace CFChat.Models
{
    /// <summary>
    /// Chat file. E.g. Image
    /// </summary>
    public class ChatFile
    {
        public string ConversationId { get; set; } = String.Empty;

        public string SenderName { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

        public byte[] Content { get; set; } = new byte[0];
    }
}
