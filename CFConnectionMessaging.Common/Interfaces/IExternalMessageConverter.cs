using CFConnectionMessaging.Models;

namespace CFConnectionMessaging.Interfaces
{
    /// <summary>
    /// Interface for converting between an external message and ConnectionMessage
    /// </summary>
    /// <typeparam name="TExternalMessage"></typeparam>
    public interface IExternalMessageConverter<TExternalMessage>
    {
        ConnectionMessage GetConnectionMessage(TExternalMessage message);

        TExternalMessage GetExternalMessage(ConnectionMessage message);
    }
}
