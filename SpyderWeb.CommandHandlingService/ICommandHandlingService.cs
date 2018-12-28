using Discord.WebSocket;
using System.Threading.Tasks;

namespace SpyderWeb.CommandHandlingService
{
    public interface ICommandHandlingService
    {
        Task MessageReceived(SocketMessage rawMessage);
    }
}
