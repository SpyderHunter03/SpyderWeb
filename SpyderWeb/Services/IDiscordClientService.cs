using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace SpyderWeb.Services
{
    public interface IDiscordClientService
    {
        Task StartClient(Func<SocketMessage, Task> messageReceivedMethod);
        DiscordSocketClient GetDiscordClient();
    }
}
