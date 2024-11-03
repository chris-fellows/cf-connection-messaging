namespace CFChat.Models
{
    /// <summary>
    /// Chat file. E.g. Image
    /// </summary>
    public class ChatFile : ChatBase
    {
        /// <summary>
        /// File name
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// File content. We could compress this but we don't
        /// </summary>
        public byte[] Content { get; set; } = new byte[0];
    }
}
