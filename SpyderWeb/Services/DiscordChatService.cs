using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SpyderWeb.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SpyderWeb.Services
{
    public class DiscordChatService : IDiscordChatService
    {
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _provider;

        public DiscordChatService(IServiceProvider provider, DiscordSocketClient discord)
        {
            _provider = provider;
            _discord = discord;
        }

        public async Task LogMessageToChannelAsync(string message, string channelName = "General")
        {
            var filter = _provider.GetRequiredService<IOptionsMonitor<DiscordFilter>>().CurrentValue;
            var channelOptions = filter.Channels.FirstOrDefault(c => c.Name.Equals(channelName));
            if (_discord.GetChannel(channelOptions.Id) is IMessageChannel channel)
                await channel.SendMessageAsync(message);
        }
    }
}
