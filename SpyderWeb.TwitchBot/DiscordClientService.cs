using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpyderWeb.Configurations;
using SpyderWeb.MicrosoftLogging;
using System;
using System.Threading.Tasks;

namespace SpyderWeb.TwitchBot
{
    public class DiscordClientService : IDiscordClientService
    {
        private readonly DiscordSocketClient _discordClient;
        private readonly Credentials _credentials;
        private readonly ILogger _logger;

        private bool usingMicrosoftLogger = false;

        public DiscordClientService(
            ILoggerFactory loggerFactory,
            IOptionsMonitor<Credentials> credentialOptions,
            BaseDiscordClient discordClient)
        {
            _logger = loggerFactory.CreateLogger("discord");
            _credentials = credentialOptions.CurrentValue;
            _discordClient = discordClient as DiscordSocketClient;

            ConfigureService();
        }

        private void ConfigureService()
        {
            if (!usingMicrosoftLogger)
            {
                _discordClient.UseMicrosoftLogging(_logger);
                usingMicrosoftLogger = true;
            }
        }

        public DiscordSocketClient GetDiscordClient()
        {
            return _discordClient;
        }

        public async Task StartClient(Func<SocketMessage, Task> messageReceivedMethod)
        {
            _discordClient.MessageReceived += messageReceivedMethod;
            await _discordClient.LoginAsync(TokenType.Bot, _credentials.DiscordToken);
            await _discordClient.StartAsync();
        }
    }
}
