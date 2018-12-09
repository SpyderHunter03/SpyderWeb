using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpyderWeb.MicrosoftLogging;
using SpyderWeb.Options;
using SpyderWeb.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SpyderWeb.Services
{
    public class CommandHandlingService : ICommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly IDiscordClientService _discordClientService;
        private readonly DiscordFilter _filter;
        private readonly IServiceProvider _provider;
        private readonly char _discordCommandPrefix;

        public CommandHandlingService(
            IServiceProvider provider,
            IDiscordClientService discordClientService, 
            CommandService commands,
            IOptionsMonitor<DiscordFilter> filter, 
            IOptionsMonitor<Credentials> credentials,
            ILoggerFactory loggerFactory,
            ITagService tagService)
        {
            _provider = provider;
            _commands = commands;
            _discordClientService = discordClientService;
            _filter = filter.CurrentValue;
            _discordCommandPrefix = credentials.CurrentValue.DiscordPrefix[0];

            var logger = loggerFactory.CreateLogger("commands");
            _commands.Log += new LogAdapter(logger).Log;
        }

        public async Task MessageReceived(SocketMessage rawMessage)
        {
            // Ignore system messages and messages from bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;
            if (!_filter.IsWhitelisted(message.Channel)) return;

            int argPos = 0;
            if (!(message.HasMentionPrefix(_discordClientService.GetDiscordClient().CurrentUser, ref argPos) 
                || message.HasCharPrefix(_discordCommandPrefix, ref argPos)))
                return;
            
            var context = new SocketCommandContext(_discordClientService.GetDiscordClient(), message);
            var result = await _commands.ExecuteAsync(context, argPos, _provider);
            if (result.Error.HasValue && result.Error.Value != CommandError.UnknownCommand)
            {
                await context.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
    }
}
