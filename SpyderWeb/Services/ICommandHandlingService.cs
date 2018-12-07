using Discord.WebSocket;
using System.Threading.Tasks;

namespace SpyderWeb.Services
{
    public interface ICommandHandlingService
    {
        Task MessageReceived(SocketMessage rawMessage);
    }
}
