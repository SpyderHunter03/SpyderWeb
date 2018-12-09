using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace SpyderWeb.Services.Interfaces
{
    public interface IDiscordClientService
    {
        Task StartClient(Func<SocketMessage, Task> messageReceivedMethod);
        DiscordSocketClient GetDiscordClient();
    }
}
