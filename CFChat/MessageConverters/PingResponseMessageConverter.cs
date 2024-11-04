using CFChat.Constants;
using CFChat.Models;
using CFConnectionMessaging.Interfaces;
using CFConnectionMessaging.Models;

namespace CFChat.MessageConverters
{
    /// <summary>
    /// Convert between PingResponse and ConnectionMessage
    /// </summary>
    internal class PingResponseMessageConverter : IExternalMessageConverter<PingResponse>
    {
        public ConnectionMessage GetConnectionMessage(PingResponse pingResponse)
        {
            var connectionMessage = new ConnectionMessage()
            {
                Id = pingResponse.Id,
                TypeId = MessageTypeIds.PingRequest,
                Parameters = new List<ConnectionMessageParameter>()
                {
                    new ConnectionMessageParameter()
                   {
                       Name = "ConversationId",
                       Value = pingResponse.ConversationId
                   },
                   new ConnectionMessageParameter()
                   {
                       Name = "SenderName",
                       Value = pingResponse.SenderName
                   }                  
                }
            };
            return connectionMessage;
        }

        public PingResponse GetExternalMessage(ConnectionMessage connectionMessage)
        {
            var pingRequest = new PingResponse()
            {
                Id = connectionMessage.Id,
                ConversationId = connectionMessage.Parameters.First(p => p.Name == "ConversationId").Value,
                SenderName = connectionMessage.Parameters.First(p => p.Name == "SenderName").Value,               
            };

            return pingRequest;
        }
    }
}
