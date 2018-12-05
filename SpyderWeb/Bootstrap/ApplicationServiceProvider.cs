using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SpyderWeb.EmojiTools;
using SpyderWeb.Helpers;
using SpyderWeb.Options;
using SpyderWeb.Services;
using SpyderWeb.Twitch;
using TwitchLib.Api;

namespace SpyderWeb.Bootstrap
{
    public static class ApplicationServiceProvider
    {
        public static IServiceCollection ConfigureServices()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            // Add Services
            serviceCollection.AddSingleton<IDiscordClient>(new DiscordSocketClient());
            serviceCollection.AddTransient<CommandService>();
            

            // Add Options

            // Add Logging
            serviceCollection.AddLogging(x => x.AddConsole());

            // Add Database


            return new ServiceCollection()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton(config)
                .Configure<Credentials>(config)
                .Configure<DiscordFilter>(config)
                .AddSingleton(new LiteDatabase($"{SolutionHelper.GetConfigRoot()}/spyder.db"))
                .AddSingleton<TagService>()
                .AddSingleton<EmojiService>()
                .AddSingleton<DiscordChatService>()
                .AddSingleton<TwitchBot>()
                .AddSingleton<TwitchAPI>()
                .BuildServiceProvider();

            //Singleton... Same class over whole application
            //Transient... Created when needed
            //Scoped... Created for a singular request

            //Perform options like this:
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-2.1#general-options-configuration
        }
    }
}
