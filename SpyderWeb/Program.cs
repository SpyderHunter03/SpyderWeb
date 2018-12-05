using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using SpyderWeb.Bootstrap;

namespace SpyderWeb
{
    public class Program
    {
        //private static DiscordSocketClient _client;
        //private static IConfiguration _config;

        //static void Main(string[] args)
        //    => MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();

        //public static async Task MainAsync(string[] args)
        //{
        //    // DISCORD SETUP
        //    _client = new DiscordSocketClient();
        //    _config = ApplicationConfigurationProvider.BuildConfig();

        //    // SERVICES SETUP
        //    var services = ApplicationServiceProvider.ConfigureServices(_client, _config);
        //    await services.GetRequiredService<CommandHandlingService>().InitializeAsync(services);
        //    await services.GetRequiredService<TagService>().BuildTagsAsync();
        //    services.GetRequiredService<EmojiService>().BuildEmojiDictionary();

        //    var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("discord");
        //    _client.UseMicrosoftLogging(logger);

        //    var options = services.GetRequiredService<IOptions<Options.Options>>().Value;

        //    // TWITCH SETUP
        //    await services.GetRequiredService<Bot>().InitializeAsync(options.TwitchBotUsername, options.TwitchSecret, options.TwitchChannel);

        //    await _client.LoginAsync(TokenType.Bot, options.Token);
        //    await _client.StartAsync();

        //    await Task.Delay(-1);
        //}

        public static void Main(string[] args)
        {
            //create service collection
            var serviceCollection = ApplicationServiceProvider.ConfigureServices();


            //create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            //entry to run app
            serviceProvider.GetService<App>().Run();
        }
    }
}
