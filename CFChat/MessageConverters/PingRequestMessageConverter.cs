using CFChat.Constants;
using CFChat.Models;
using CFConnectionMessaging.Interfaces;
using CFConnectionMessaging.Models;

namespace CFChat.MessageConverters
{
    /// <summary>
    /// Convert between PingRequest and ConnectionMessage
    /// </summary>
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
                       Name = "ConversationId",
                       Value = pingRequest.ConversationId
                   },
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
                Id = connectionMessage.Id,                
                ConversationId = connectionMessage.Parameters.First(p => p.Name == "ConversationId").Value,
                SenderName = connectionMessage.Parameters.First(p => p.Name == "SenderName").Value,               
            };

            return pingRequest;
        }
    }
}
