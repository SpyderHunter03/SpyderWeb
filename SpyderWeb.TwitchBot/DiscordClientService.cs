using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpyderWeb.Configurations;
using System.Threading.Tasks;

namespace SpyderWeb.Discord
{
    public class DiscordClientService : IDiscordClientService
    {
        private readonly DiscordSocketClient _discordClient;
        private readonly Credentials _credentials;
        private readonly ILogger _logger;
        private readonly IDiscordCustomCommandService _discordCustomCommandService;

        private bool useMicrosoftLoggerSet = false;

        public DiscordClientService(
            ILoggerFactory loggerFactory,
            IOptionsMonitor<Credentials> credentialOptions,
            BaseDiscordClient discordClient,
            IDiscordCustomCommandService discordCustomCommandService) //Call to instantiate commandService
        {
            _logger = loggerFactory.CreateLogger("discord");
            _credentials = credentialOptions.CurrentValue;
            _discordClient = discordClient as DiscordSocketClient;
            _discordCustomCommandService = discordCustomCommandService;

            ConfigureService();
        }

        private void ConfigureService()
        {
            if (!useMicrosoftLoggerSet)
            {                
                _discordClient.UseMicrosoftLogging(_logger);
                useMicrosoftLoggerSet = true;
            }
        }

        public async Task StartClient()
        {
            _discordClient.MessageReceived += _discordCustomCommandService.MessageReceived;
            await _discordClient.LoginAsync(TokenType.Bot, _credentials.DiscordToken);
            await _discordClient.StartAsync();
        }
    }

    public interface IDiscordClientService
    {
        Task StartClient();
    }
}
