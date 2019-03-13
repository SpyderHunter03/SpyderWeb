using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using SpyderWeb.Configurations;
using System.Linq;
using System.Threading.Tasks;

namespace SpyderWeb.DiscordMessageSender
{
    public class DiscordChatService : IDiscordChatService
    {
        private readonly DiscordSocketClient _discord;
        private readonly DiscordFilter _discordFilter;

        public DiscordChatService(IOptionsMonitor<DiscordFilter> optionsMonitor, DiscordSocketClient discord)
        {
            _discord = discord;
            _discordFilter = optionsMonitor.CurrentValue;
        }

        public async Task LogMessageToChannelAsync(string message, string channelName = "General")
        {
            // TODO: Figure out a way to not use _discordFilter.Channels
            //var channelOptions = _discordFilter.Channels.FirstOrDefault(c => c.Name.Equals(channelName));
            // if (_discord.GetChannel(channelOptions.Id) is IMessageChannel channel)
            //     await channel.SendMessageAsync(message);
        }
    }
}
