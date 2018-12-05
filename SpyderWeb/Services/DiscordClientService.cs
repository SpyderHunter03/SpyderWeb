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
        public DiscordSocketClient DiscordClient { get; private set; }
        private Credentials _credentials;

        private readonly ILogger _logger;
        private readonly IOptions<Credentials> _credentialOptions;
        
        public DiscordClientService(ILoggerFactory loggerFactory, IOptions<Credentials> credentialOptions)
        {
            _logger = loggerFactory.CreateLogger("discord");
            _credentialOptions = credentialOptions;

            ConfigureService();
        }

        private void ConfigureService()
        {
            if (DiscordClient == null)
            {
                DiscordClient = new DiscordSocketClient();

                DiscordClient.UseMicrosoftLogging(_logger);

                _credentials = _credentialOptions.Value;
            }
        }

        public async Task StartClient()
        {
            await DiscordClient.LoginAsync(Discord.TokenType.Bot, _credentials.DiscordToken);
            await DiscordClient.StartAsync();
        }
    }
}
