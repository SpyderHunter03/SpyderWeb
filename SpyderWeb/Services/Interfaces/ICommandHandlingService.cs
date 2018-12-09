using Discord.WebSocket;
using System.Threading.Tasks;

namespace SpyderWeb.Services.Interfaces
{
    public interface ICommandHandlingService
    {
        Task MessageReceived(SocketMessage rawMessage);
    }
}
