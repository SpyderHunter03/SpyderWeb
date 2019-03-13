using AutoMapper;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SpyderWeb.Configurations;
using SpyderWeb.Database;
using SpyderWeb.Discord;
using SpyderWeb.DiscordMessageSender;
using SpyderWeb.EmojiTools;
using SpyderWeb.FacebookCore;
using SpyderWeb.FacebookCore.Interfaces;
using SpyderWeb.Models;
using SpyderWeb.Overwatch;
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
            serviceCollection.AddSingleton<IDiscordClientService, DiscordClientService>();
            serviceCollection.AddTransient<IApiSettings, ApiSettings>();
            serviceCollection.AddTransient<ITwitchAPI, TwitchAPI>();
            serviceCollection.AddSingleton<ITwitchBot, Twitch.TwitchBot>();
            serviceCollection.AddTransient<IDiscordChatService, DiscordChatService>();
            serviceCollection.AddSingleton<IEmojiService, EmojiService>();
            serviceCollection.AddSingleton<IDiscordCustomCommandService, DiscordCustomCommandService>();
            serviceCollection.AddTransient<IFacebookClient, FacebookClient>();
            serviceCollection.AddTransient<IFacebookService, FacebookService>();
            serviceCollection.AddTransient<IOverwatchService, OverwatchService>();

            // Add Discord
            serviceCollection.AddSingleton<BaseDiscordClient, DiscordSocketClient>();
            serviceCollection.AddSingleton<CommandService>();

            // Add Options
            serviceCollection.AddOptions();
            serviceCollection.Configure<Credentials>(configuration);
            serviceCollection.Configure<DiscordFilter>(configuration);

            // Add Logging
            serviceCollection.AddLogging(x => x.AddConsole());

            // Add Database
            serviceCollection.AddTransient<IDatabaseService<Tag>, LiteDatabaseService<Tag>>();
            serviceCollection.AddTransient<IDatabaseService<TwitchUser>, LiteDatabaseService<TwitchUser>>();

            // Add AutoMapper
            // var mappingConfig = new MapperConfiguration(mc =>
            // {
            //     mc.AddProfile(new OverwatchProfile());
            // });

            // IMapper mapper = mappingConfig.CreateMapper();
            // serviceCollection.AddSingleton(mapper);

            return serviceCollection;

            //Singleton... Same class over whole application
            //Transient... Created when needed
            //Scoped... Created for a singular request

            //Perform options like this:
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-2.1#general-options-configuration
        }
    }
}
