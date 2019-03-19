using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpyderWeb.Configurations;
using SpyderWeb.CoreModules;
using SpyderWeb.Database;
using SpyderWeb.Database.Models;
using SpyderWeb.Events;
using SpyderWeb.Events.EventArguments;
using System;
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

            await Task.Run(() =>
            {
                // var logMessage = 
                //     $"Message Received: {context.Message.Content}" + Environment.NewLine + 
                //     $"Message Sent By: {context.User.Username} ({context.User.Id})" + Environment.NewLine + 
                //     $"From Channel: {context.Channel.Name} ({context.Channel.Id})" + Environment.NewLine + 
                //     $"From Server/Guild: {context.Guild.Name} ({context.Guild.Id})";
                // _logger.LogInformation(logMessage);

                /* Send message to all modules */
                EventPublisher.OnMessageReceivedEvent(context, new MessageReceivedArgs(_credentials.DiscordPrefix, context.Message.Content, context));
                
                /* return result to context.Channel.SendMessageAsync if there is one */

                /* If there is an error then I want to send the error to: */
                /* return result to context.Channel.SendMessageAsync if there is one */
            });
        }

        public async void SendMessageAsync(object sender, SendMessageArgs args)
        {
            var context = sender as SocketCommandContext;
            if (context == null) return;

            await context.Channel.SendMessageAsync(args.Message);
        }

        // public async void BuildTagsAsync(object sender, EventArgs args)
        // {
        //     if (TagsModule == null)
        //         await _commands.RemoveModuleAsync(TagsModule);

        //     var tags = _database.GetAll();

        //     TagsModule = await _commands.CreateModuleAsync("", module =>
        //     {
        //         foreach (var tag in tags)
        //         {
        //             module.AddCommand(tag.Name, (context, @params, provider, command) =>
        //             {
        //                 return context.Channel.SendMessageAsync(
        //                     $"{tag.Name}: {tag.Content}");
        //             },
        //             command => { });
        //         }
        //     });

        //     _logger.LogInformation($"Build {tags.Count()} tags successfully.");
        // }

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
    }

    public interface IDiscordClientService
    {
        Task StartClient();
    }
}
