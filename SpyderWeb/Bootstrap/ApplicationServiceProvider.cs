using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SpyderWeb.EmojiTools;
using SpyderWeb.Options;
using SpyderWeb.Services;
using SpyderWeb.Twitch;
using TwitchLib.Api;
using TwitchLib.Api.Core;
using TwitchLib.Api.Core.Interfaces;
using TwitchLib.Api.Interfaces;

namespace SpyderWeb.Bootstrap
{
    public static class ApplicationServiceProvider
    {
        public static IServiceCollection ConfigureServices()
        {
            var configuration = ApplicationConfigurationProvider.BuildConfig();
            IServiceCollection serviceCollection = new ServiceCollection();

            // Add Services
            serviceCollection.AddTransient<IApp, App>();
            serviceCollection.AddTransient<IDiscordClientService, DiscordClientService>();
            serviceCollection.AddSingleton<BaseDiscordClient, DiscordSocketClient>();
            serviceCollection.AddTransient<ICommandHandlingService, CommandHandlingService>();
            serviceCollection.AddTransient<IApiSettings, ApiSettings>(); //add user options here or create my own and pass in options and populate then
            serviceCollection.AddTransient<ITwitchAPI, TwitchAPI>();
            serviceCollection.AddSingleton<ITwitchBot, TwitchBot>();
            serviceCollection.AddTransient<IDiscordChatService, DiscordChatService>();
            serviceCollection.AddSingleton<IEmojiService, EmojiService>();
            serviceCollection.AddSingleton<ITagService, TagService>();
            serviceCollection.AddSingleton<CommandService>();

            // Add Options
            serviceCollection.Configure<Credentials>(configuration);
            serviceCollection.Configure<DiscordFilter>(configuration);

            // Add Logging
            serviceCollection.AddLogging(x => x.AddConsole());

            // Add Database
            serviceCollection.AddTransient<IDatabaseService, LiteDatabaseService>();

            return serviceCollection;

            //Singleton... Same class over whole application
            //Transient... Created when needed
            //Scoped... Created for a singular request

            //Perform options like this:
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-2.1#general-options-configuration
        }
    }
}
