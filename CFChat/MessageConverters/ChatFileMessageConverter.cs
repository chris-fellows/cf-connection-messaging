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
    /// Convert between ChatFile and ConnectionMessage
    /// </summary>
    internal class ChatFileMessageConverter : IExternalMessageConverter<ChatFile>
    {
        public ConnectionMessage GetConnectionMessage(ChatFile chatFile)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = chatFile.Id,
                TypeId = MessageTypeIds.ChatFile,
                Parameters = new List<ConnectionMessageParameter>()
                {
                   new ConnectionMessageParameter()
                   {
                       Name = "SenderName",
                       Value = chatFile.SenderName
                   },
                   new ConnectionMessageParameter()
                   {
                       Name = "Name",
                       Value = chatFile.Name
                   },
                   new ConnectionMessageParameter()
                   {
                       Name = "Content",
                       Value = Convert.ToBase64String(chatFile.Content)
                   }
                }
            };
            return connectionMessage;
        }

        public ChatFile GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var chatFile = new ChatFile()
            {
                SenderName = connectionMessage.Parameters.First(p => p.Name == "SenderName").Value,
                Name = connectionMessage.Parameters.First(p => p.Name == "Name").Value,
                Content = Convert.FromBase64String(connectionMessage.Parameters.First(p => p.Name == "Content").Value)
            };

            return chatFile;
        }
    }
}
