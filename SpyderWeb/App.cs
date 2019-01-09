using SpyderWeb.Discord;

namespace SpyderWeb
{
    public class App : IApp
    {
        private readonly IDiscordClientService _discordClientService;

        public App(IDiscordClientService discordClientService)
        {
            _discordClientService = discordClientService;
        }

        public async void Run()
        {
            await _discordClientService.StartClient();
        }
    }
}
