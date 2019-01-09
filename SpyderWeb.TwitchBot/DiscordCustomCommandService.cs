﻿using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpyderWeb.Configurations;
using SpyderWeb.Database;
using SpyderWeb.Events;
using SpyderWeb.Events.EventArguments;
using SpyderWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SpyderWeb.Discord
{
    public class DiscordCustomCommandService : IDiscordCustomCommandService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discordClient;
        private readonly IDatabaseService<Tag> _database;
        private readonly DiscordFilter _filter;
        private readonly char _discordCommandPrefix;
        private readonly ILogger _logger;
        private List<Assembly> _addedAssemblies = new List<Assembly>();

        public Func<LogMessage,Task> Log { get; set; }
        public ModuleInfo TagsModule { get; set; }

        public DiscordCustomCommandService(
            IServiceProvider serviceProvider,
            CommandService commands,
            BaseDiscordClient discordClient,
            IDatabaseService<Tag> database,
            IOptionsMonitor<DiscordFilter> filter,
            IOptionsMonitor<Credentials> credentials,
            ILoggerFactory loggerFactory)
        {
            _serviceProvider = serviceProvider;
            _commands = commands;
            _discordClient = discordClient as DiscordSocketClient;
            _database = database;
            _filter = filter.CurrentValue;
            _discordCommandPrefix = credentials.CurrentValue.DiscordPrefix[0];

            _logger = loggerFactory.CreateLogger("DiscordCustomCommandService");
            _commands.Log += new DiscordLogAdapter(_logger).Log;

            Init();
        }

        private async void Init()
        {
            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider);
            EventPublisher.CreateTagEvent -= BuildTagsAsync;
            EventPublisher.UpdateTagEvent -= BuildTagsAsync;
            EventPublisher.DeleteTagEvent -= BuildTagsAsync;
            EventPublisher.CreateTagEvent += BuildTagsAsync;
            EventPublisher.UpdateTagEvent += BuildTagsAsync;
            EventPublisher.DeleteTagEvent += BuildTagsAsync;
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

        public async Task MessageReceived(SocketMessage rawMessage)
        {
            // Ignore system messages and messages from bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;
            if (!_filter.IsWhitelisted(message.Channel)) return;

            int argPos = 0;
            if (!(message.HasMentionPrefix(_discordClient.CurrentUser, ref argPos)
                || message.HasCharPrefix(_discordCommandPrefix, ref argPos)))
                return;

            var context = new SocketCommandContext(_discordClient, message);

            await Task.Run(async () =>
            {
                var result = await _commands.ExecuteAsync(context, argPos, _serviceProvider);
                if (result.Error.HasValue && result.Error.Value != CommandError.UnknownCommand)
                    await context.Channel.SendMessageAsync(result.ErrorReason);
            });
        }
    }

    public interface IDiscordCustomCommandService
    {
        ModuleInfo TagsModule { get; }
        void BuildTagsAsync(object sender, EventArgs args);
        Task MessageReceived(SocketMessage rawMessage);
    }
}
