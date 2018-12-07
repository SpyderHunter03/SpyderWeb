using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpyderWeb.MicrosoftLogging;
using SpyderWeb.Options;
using System.Threading.Tasks;

namespace SpyderWeb.Services
{
    public class DiscordClientService : IDiscordClientService
    {
        private readonly DiscordSocketClient _discordClient;
        private readonly Credentials _credentials;
        private readonly ILogger _logger;

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
            _discordClient.UseMicrosoftLogging(_logger);
        }

        public async Task StartClient()
        {
            await _discordClient.LoginAsync(TokenType.Bot, _credentials.DiscordToken);
            await _discordClient.StartAsync();
        }

        public DiscordSocketClient GetDiscordClient()
        {
            return _discordClient;
        }
    }
}
