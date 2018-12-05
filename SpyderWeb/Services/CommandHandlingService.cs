using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpyderWeb.MicrosoftLogging;
using SpyderWeb.Options;
using System;
using System.Threading.Tasks;

namespace SpyderWeb.Services
{
    public class CommandHandlingService : ICommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly Credentials _options;
        private readonly IDiscordClientService _discordClientService;
        private readonly Filter _filter;
        private IServiceProvider _provider;

        public CommandHandlingService(
            IDiscordClientService discordClientService, 
            CommandService commands, 
            IOptions<Filter> filter, 
            IOptions<Credentials> options,
            ILoggerFactory loggerFactory)
        {
            _commands = commands;
            _discordClientService = discordClientService;
            _filter = filter.Value;
            _options = options.Value;

            var logger = loggerFactory.CreateLogger("commands");
            _commands.Log += new LogAdapter(logger).Log;

            _discordClientService.DiscordClient.MessageReceived += MessageReceived;
        }

        public async Task MessageReceived(SocketMessage rawMessage)
        {
            // Ignore system messages and messages from bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;
            if (!_filter.IsWhitelisted(message.Channel)) return;

            int argPos = 0;
            if (!(message.HasMentionPrefix(_discordClientService.DiscordClient.CurrentUser, ref argPos) 
                || message.HasStringPrefix(_options.DiscordPrefix, ref argPos)))
                return;

            var context = new SocketCommandContext(_discordClientService.DiscordClient, message);
            var result = await _commands.ExecuteAsync(context, argPos, _provider);
            if (result.Error.HasValue && result.Error.Value != CommandError.UnknownCommand)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }
    }
}
