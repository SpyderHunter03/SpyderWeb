using Discord.WebSocket;
using System.Threading.Tasks;

namespace SpyderWeb.Services
{
    interface ICommandHandlingService
    {
        Task MessageReceived(SocketMessage rawMessage);
    }
}
