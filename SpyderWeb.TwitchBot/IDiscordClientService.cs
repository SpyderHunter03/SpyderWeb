using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace SpyderWeb.TwitchBot
{
    public interface IDiscordClientService
    {
        Task StartClient(Func<SocketMessage, Task> messageReceivedMethod);
        DiscordSocketClient GetDiscordClient();
    }
}