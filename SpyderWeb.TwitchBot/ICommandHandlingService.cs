using Discord.WebSocket;
using System.Threading.Tasks;

namespace SpyderWeb.TwitchBot
{
    public interface ICommandHandlingService
    {
        Task MessageReceived(SocketMessage rawMessage);
    }
}
