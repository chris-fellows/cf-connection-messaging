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
    internal class PingRequestMessageConverter : IExternalMessageConverter<PingRequest>
    {
        public ConnectionMessage GetConnectionMessage(PingRequest pingRequest)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = pingRequest.Id,
                TypeId = MessageTypeIds.PingRequest,
                Parameters = new List<ConnectionMessageParameter>()
                {
                   new ConnectionMessageParameter()
                   {
                       Name = "SenderName",
                       Value = pingRequest.SenderName
                   }                  
                }
            };
            return connectionMessage;
        }

        public PingRequest GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var pingRequest = new PingRequest()
            {
                SenderName = connectionMessage.Parameters.First(p => p.Name == "SenderName").Value,               
            };

            return pingRequest;
        }
    }
}
