using SpyderWeb.Services;
using System.Threading.Tasks;

namespace SpyderWeb
{
    public class App
    {
        private readonly IDiscordClientService _discordClientService;
        public App(IDiscordClientService discordClientService)
        {
            _discordClientService = discordClientService;
        }

        public async void Run()
        {
            await _discordClientService.StartClient();

            await Task.Delay(-1);
        }
    }
}
