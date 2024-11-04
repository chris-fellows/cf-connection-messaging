using CFChat.Constants;
using CFChat.Models;
using CFConnectionMessaging.Interfaces;
using CFConnectionMessaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFChat.MessageConverters
{
    /// <summary>
    /// Convert between ChatMessage and ConnectionMessage
    /// </summary>
    internal class ChatMessageMessageConverter : IExternalMessageConverter<ChatMessage>
    {
        public ConnectionMessage GetConnectionMessage(ChatMessage chatMessage)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = chatMessage.Id,
                TypeId = MessageTypeIds.ChatMessage,                
                Parameters = new List<ConnectionMessageParameter>()
                {
                          new ConnectionMessageParameter()
                   {
                       Name = "ConversationId",
                       Value = chatMessage.ConversationId
                   },
                    new ConnectionMessageParameter()
                   {
                       Name = "SenderName",
                       Value = chatMessage.SenderName
                   },
                   new ConnectionMessageParameter()
                   {
                       Name = "Text",
                       Value = chatMessage.Text
                   }                  
                }
            };
            return connectionMessage;
        }

        public ChatMessage GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var chatMessage = new ChatMessage()
            {
                Id = connectionMessage.Id,               
                ConversationId = connectionMessage.Parameters.First(p => p.Name == "ConversationId").Value,
                SenderName = connectionMessage.Parameters.First(p => p.Name == "SenderName").Value,
                Text = connectionMessage.Parameters.First(p => p.Name == "Text").Value
            };
            return chatMessage;
        }
    }
}
