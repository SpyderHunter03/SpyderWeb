using Discord.WebSocket;
using System.Threading.Tasks;

namespace SpyderWeb.Services
{
    public interface IDiscordClientService
    {
        DiscordSocketClient DiscordClient { get; }
        Task StartClient();
    }
}
