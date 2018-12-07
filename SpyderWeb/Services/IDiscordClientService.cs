using Discord.WebSocket;
using System.Threading.Tasks;

namespace SpyderWeb.Services
{
    public interface IDiscordClientService
    {
        Task StartClient();
        DiscordSocketClient GetDiscordClient();
    }
}
