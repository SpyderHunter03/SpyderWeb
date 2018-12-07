using SpyderWeb.Services;

namespace SpyderWeb
{
    public class App : IApp
    {
        private readonly IDiscordClientService _discordClientService;
        private readonly ICommandHandlingService _commandHandlingService;

        public App(
            IDiscordClientService discordClientService, 
            ICommandHandlingService commandHandlingService)
        {
            _discordClientService = discordClientService;
            _commandHandlingService = commandHandlingService;
        }

        public async void Run()
        {
            await _discordClientService.StartClient(_commandHandlingService.MessageReceived);
        }
    }
}
