using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpyderWeb.Configurations;
using System;
using System.Threading.Tasks;

namespace SpyderWeb.Discord
{
    public class CommandHandlingService : ICommandHandlingService
    {
        private readonly DiscordSocketClient _discordClient;
        private readonly IDiscordCommandService _discordCommandService;
        private readonly DiscordFilter _filter;
        private readonly char _discordCommandPrefix;

        public CommandHandlingService(
            BaseDiscordClient discordClient,
            IDiscordCommandService discordCommandService,
            IOptionsMonitor<DiscordFilter> filter,
            IOptionsMonitor<Credentials> credentials,
            ILoggerFactory loggerFactory)
        {
            _discordClient = discordClient as DiscordSocketClient;
            _discordCommandService = discordCommandService;
            _filter = filter.CurrentValue;
            _discordCommandPrefix = credentials.CurrentValue.DiscordPrefix[0];

            var logger = loggerFactory.CreateLogger("CommandHandlingService");
            _discordCommandService.AddLog(new DiscordLogAdapter(logger).Log);
        }

        public async Task MessageReceived(SocketMessage rawMessage)
        {
            // Ignore system messages and messages from bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;
            if (!_filter.IsWhitelisted(message.Channel)) return;

            int argPos = 0;
            if (!(message.HasMentionPrefix(_discordClient.CurrentUser, ref argPos)
                || message.HasCharPrefix(_discordCommandPrefix, ref argPos)))
                return;

            var context = new SocketCommandContext(_discordClient, message);
            var result = await _discordCommandService.ExecuteAsync(context, argPos);
            if (result.Error.HasValue && result.Error.Value != CommandError.UnknownCommand)
            {
                await context.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
    }

    public interface ICommandHandlingService
    {
        Task MessageReceived(SocketMessage rawMessage);
    }
}
