using AutoMapper;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpyderWeb.Configurations;
using SpyderWeb.CoreModules;
using SpyderWeb.CoreModules.Models;
using SpyderWeb.Database;
using SpyderWeb.Database.Models;
using SpyderWeb.Events;
using SpyderWeb.Events.EventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpyderWeb.Discord
{
    public class DiscordClientService : IDiscordClientService
    {
        private readonly DiscordSocketClient _discordClient;
        private readonly Credentials _credentials;
        private readonly IDatabaseService<DiscordServer> _discordServerDatabase;
        private readonly IDatabaseService<User> _userDatabase;
        private readonly ILogger _logger;

        private bool useMicrosoftLoggerSet = false;

        public DiscordClientService(
            ILoggerFactory loggerFactory,
            IOptionsMonitor<Credentials> credentialOptions,
            IDatabaseService<DiscordServer> discordServerDatabase,
            IDatabaseService<User> userDatabase,
            BaseDiscordClient discordClient)
        {
            _logger = loggerFactory.CreateLogger("discord");
            _credentials = credentialOptions.CurrentValue;
            _discordServerDatabase = discordServerDatabase;
            _userDatabase = userDatabase;
            _discordClient = discordClient as DiscordSocketClient;

            ConfigureService();
        }

        private void ConfigureService()
        {
            if (!useMicrosoftLoggerSet)
            {                
                _discordClient.UseMicrosoftLogging(_logger);
                useMicrosoftLoggerSet = true;
            }
            // TODO: Move InfoModule registration somewhere else
            EventPublisher.MessageReceivedEvent += InfoModule.MessageReceived;
            EventPublisher.SendMessageEvent += SendMessageAsync;
        }

        public async Task StartClient()
        {
            SetupListeners();

            await _discordClient.LoginAsync(TokenType.Bot, _credentials.DiscordToken);
            await _discordClient.StartAsync();
        }

        private void SetupListeners()
        {
            _discordClient.MessageReceived += MessageReceived;
            _discordClient.Connected += Connected;
            _discordClient.GuildAvailable += GuildAvailable;
            _discordClient.JoinedGuild += JoinedGuild;
        }

        public async Task Connected()
        {
            await Task.Run(() => 
            {
                _logger.LogDebug("SpyderWeb Bot has connected.");
            });
        }

        public async Task GuildAvailable(SocketGuild guild)
        {
            if (guild != null)
            {
                await Task.Run(() => 
                {
                    var owner = GetServerOwner(guild.OwnerId, guild.Owner.Username);
                    var discordServer = GetServer(guild.Id, guild.Name, owner);

                    _logger.LogDebug("Guild Available" + Environment.NewLine
                    + $"Guild: {discordServer.ServerName}({discordServer.ServerId})" + Environment.NewLine
                    + $"Owner: {owner.DiscordName}({owner.DiscordId})");
                });
            }
        }

        public async Task JoinedGuild(SocketGuild guild)
        {
            if (guild != null)
            {
                await Task.Run(() => 
                {
                    _logger.LogDebug("Joined Guild" + Environment.NewLine
                    + $"Guild: {guild.Name}({guild.Id})" + Environment.NewLine
                    + $"Owner: {guild.Owner.Nickname}({guild.OwnerId})");
                });
            }
        }

        public async Task MessageReceived(SocketMessage rawMessage)
        {
            // Ignore system messages
            if (!(rawMessage is SocketUserMessage message)) return;
            // Ignore bot messages
            if (message.Source != MessageSource.User) return;
            // Ignore messages from channels outside of the whitelisted ones
            //if (!_filter.IsWhitelisted(message.Channel)) return; // Move to database for filters

            int argPos = 0;
            if (!(message.HasMentionPrefix(_discordClient.CurrentUser, ref argPos)
                || message.HasStringPrefix(_credentials.DiscordPrefix, ref argPos)))
                return;

            var context = new SocketCommandContext(_discordClient, message);

            await Task.Run(async () =>
            {                
                /* Send message to all modules */
                var app = await context.Client.GetApplicationInfoAsync();

                var discordContext = new DiscordContext
                {
                    AppOwner = new DiscordContext.DiscordObject { Id = app.Owner.Id, Name = app.Owner.Username },
                    Channel = new DiscordContext.DiscordObject { Id = context.Channel.Id, Name = context.Channel.Name },
                    Guild = new DiscordContext.DiscordObject { Id = context.Guild.Id, Name = context.Guild.Name },
                    Guilds = context.Client.Guilds.Select(g => new DiscordContext.DiscordStats {
                        Channels = g.Channels.Select(c => new DiscordContext.DiscordObject {
                            Id = c.Id,
                            Name = c.Name
                        }),
                        Users = g.Users.Select(u => new DiscordContext.DiscordObject {
                            Id = u.Id,
                            Name = u.Username
                        }),
                    }),
                    Version = DiscordConfig.Version
                };

                EventPublisher.OnMessageReceivedEvent(context, 
                    new MessageReceivedArgs((int)MessageSender.Discord, _credentials.DiscordPrefix, 
                                            context.Message.Content, discordContext));
            });
        }

        public async void SendMessageAsync(object sender, SendMessageArgs args)
        {
            var context = sender as SocketCommandContext;
            if (context == null) return;

            await context.Channel.SendMessageAsync(args.Message);
        }

        private User GetServerOwner(ulong ownerId, string ownerUsername)
        {
            var owner = _userDatabase.GetAll().SingleOrDefault(u => u.DiscordId == ownerId);
            if (owner == null)
            {
                owner = new User 
                {
                    DiscordId = ownerId,
                    DiscordName = ownerUsername,
                    CreatedAt = DateTimeOffset.UtcNow
                };

                _userDatabase.Add(owner);
            }

            return owner;
        }

        private DiscordServer GetServer(ulong serverId, string serverName, User owner)
        {
            var discordServer = _discordServerDatabase.GetAll().SingleOrDefault(ds => ds.ServerId == serverId);
            if (discordServer == null)
            {
                discordServer = new DiscordServer
                {
                    ServerId = serverId,
                    ServerName = serverName,
                    OwnerId = owner.Id,
                    CreatedAt = DateTimeOffset.UtcNow
                };

                _discordServerDatabase.Add(discordServer);
            }

            return discordServer;
        }

        /*
        /// <summary>
        /// This looks for Assemblies that have been already loaded... 
        /// But if they aren't directly referenced, then they won't be loaded
        /// Looking to move to .Net Core MEF solution
        /// </summary>
        /// <returns></returns>

        private async void Init()
        {
            var all =AppDomain.CurrentDomain.GetAssemblies()
                        .Where(assembly => assembly.FullName.StartsWith("SpyderWeb"))
                        .SelectMany(x => x.DefinedTypes)
                        .Where(type => type.IsClass && !type.IsAbstract && typeof(SpyderModuleBase).IsSubclassOf(type))
                        .ToList();
                        
            foreach (var type in all)
            {
                await _commands.AddModuleAsync(type, _serviceProvider);
            }
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.ManifestModule.Name.StartsWith("SpyderWeb"));
            Add all modules in the solution
            foreach (Assembly a in assemblies)
            {
               if (a.GetTypes().Any(t => t.IsSubclassOf(typeof(SpyderModuleBase))))
                   await _commands.AddModulesAsync(a, _serviceProvider);
            }

            await BuildTagsAsync();
        }

        public async void BuildTagsAsync(object sender, EventArgs args)
        {
            if (TagsModule == null)
                await _commands.RemoveModuleAsync(TagsModule);

            var tags = _database.GetAll();

            TagsModule = await _commands.CreateModuleAsync("", module =>
            {
                foreach (var tag in tags)
                {
                    module.AddCommand(tag.Name, (context, @params, provider, command) =>
                    {
                        return context.Channel.SendMessageAsync(
                            $"{tag.Name}: {tag.Content}");
                    },
                    command => { });
                }
            });

            _logger.LogInformation($"Build {tags.Count()} tags successfully.");
        }
        
        */
    }

    public interface IDiscordClientService
    {
        Task StartClient();
    }
}
