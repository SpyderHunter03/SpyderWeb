using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpyderWeb.Configurations;
using SpyderWeb.CoreModules;
using SpyderWeb.Events;
using SpyderWeb.Events.EventArguments;
using System.Threading.Tasks;

namespace SpyderWeb.Discord
{
    public class DiscordClientService : IDiscordClientService
    {
        private readonly DiscordSocketClient _discordClient;
        private readonly Credentials _credentials;
        private readonly ILogger _logger;

        private bool useMicrosoftLoggerSet = false;

        public DiscordClientService(
            ILoggerFactory loggerFactory,
            IOptionsMonitor<Credentials> credentialOptions,
            BaseDiscordClient discordClient)
        {
            _logger = loggerFactory.CreateLogger("discord");
            _credentials = credentialOptions.CurrentValue;
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
            EventPublisher.MessageReceivedEvent += InfoModuleV2.MessageReceivedAsync;
            EventPublisher.SendMessageEvent += SendMessageAsync;
        }

        public async Task StartClient()
        {
            _discordClient.MessageReceived += MessageReceived;
            await _discordClient.LoginAsync(TokenType.Bot, _credentials.DiscordToken);
            await _discordClient.StartAsync();
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
                || message.HasCharPrefix(_credentials.DiscordPrefix[0], ref argPos)))
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
                EventPublisher.OnMessageReceivedEvent(context, new MessageReceivedArgs(context.Message.Content, context));
                
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
    }

    public interface IDiscordClientService
    {
        Task StartClient();
    }
}
