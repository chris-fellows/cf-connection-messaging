using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFChat.Models
{
    /// <summary>
    /// Base class for chat messages
    /// </summary>
    public abstract class ChatBase
    {
        /// <summary>
        /// Unique message Id
        /// </summary>
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Conversation Id
        /// </summary>
        public string ConversationId { get; set; } = String.Empty;

        /// <summary>
        /// Sender name
        /// </summary>
        public string SenderName { get; set; } = String.Empty;

    }
}
