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
                Id = Guid.NewGuid().ToString(),
                TypeId = MessageTypeIds.ChatMessage,
                //SenderIP = "",
                //SenderPort = 0,
                Parameters = new List<ConnectionMessageParameter>()
                {
                    new ConnectionMessageParameter()
                   {
                       Name = "SenderName",
                       Value = chatMessage.SenderName
                   },
                   new ConnectionMessageParameter()
                   {
                       Name = "Text",
                       Value = chatMessage.Text
                   },
                   new ConnectionMessageParameter()
                   {
                       Name = "Encrypted",
                       Value = "0"
                   }
                }
            };
            return connectionMessage;
        }

        public ChatMessage GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var chatMessage = new ChatMessage()
            {
                SenderName = connectionMessage.Parameters.First(p => p.Name == "SenderName").Value,
                Text = connectionMessage.Parameters.First(p => p.Name == "Text").Value
            };
            return chatMessage;
        }
    }
}
