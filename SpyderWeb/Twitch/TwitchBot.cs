using LiteDB;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpyderWeb.Data.User;
using SpyderWeb.Options;
using SpyderWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Api.Interfaces;
using TwitchLib.Api.Services;
using TwitchLib.Api.Services.Events;
using TwitchLib.Api.Services.Events.FollowerService;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace SpyderWeb.Twitch
{
    public class TwitchBot : ITwitchBot
    {
        private TwitchClient _client;
        private FollowerService _service;
        private readonly ILogger _logger;
        private readonly IDiscordChatService _discordChatService;
        private readonly ITwitchAPI _twitchAPI;
        private readonly Credentials _options;
        private readonly LiteDatabase _database;

        public TwitchBot(
            ILoggerFactory loggerFactory,
            IDiscordChatService discordChatService,
            ITwitchAPI twitchApi,
            IOptionsMonitor<Credentials> options, 
            LiteDatabase database)
        {
            _logger = loggerFactory.CreateLogger("twitch");
            _discordChatService = discordChatService;
            _twitchAPI = twitchApi;
            _options = options.CurrentValue;
            _database = database;
        }

        public async Task InitializeAsync(string userName, string oauthToken, string channel)
        {
            var credentials = new ConnectionCredentials(userName, oauthToken);
            _client = new TwitchClient();
            _client.Initialize(credentials, channel);

            _client.OnLog += OnLog;
            _client.OnConnectionError += OnConnectionError;
            _client.OnConnected += OnConnected;
            _client.OnJoinedChannel += OnJoinedChannel;
            _client.OnNewSubscriber += OnNewSubscriber;
            //_client.OnMessageReceived += OnMessageReceived;

            _twitchAPI.Settings.ClientId = _options.TwitchClientId;
            _twitchAPI.Settings.AccessToken = _options.TwitchSecret;
            _service = new FollowerService(_twitchAPI);
            _service.SetChannelsByName(new List<string> { "SpyderHunter03" });
            _service.OnServiceStarted += OnServiceStarted;
            _service.OnServiceTick += OnServiceTick;
            _service.OnNewFollowersDetected += OnNewFollowersDetected;

            await Task.Run(() => _client.Connect());
            _logger.LogInformation($"Got passed Connect");
            await Task.Run(() => _service.Start());
            _logger.LogInformation($"Got passed Start");
        }

        private void OnLog(object sender, OnLogArgs e)
        {
            _logger.LogInformation($"{e.Data}");
        }

        private void OnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            _logger.LogError($"There was an error connecting. {e.Error}");
        }

        private void OnConnected(object sender, OnConnectedArgs e)
        {
            _logger.LogInformation($"Connected to {e.AutoJoinChannel}");
        }

        private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            _logger.LogInformation("Hey guys! I am a bot connected via TwitchLib!");
            _client.SendMessage(e.Channel, "Hey guys! I am a bot connected via TwitchLib!");
            Task.Run(async () => await _discordChatService.LogMessageToChannelAsync("Hey guys! I am a bot connected via TwitchLib!", "TestBotChannel"));
        }

        private void OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            _logger.LogInformation($"We had a new subscriber! Thank you for subbing {e.Subscriber.DisplayName}.");
            _client.SendMessage(e.Channel, $"We had a new subscriber! Thank you for subbing {e.Subscriber.DisplayName}.");
            Task.Run(async () => await _discordChatService.LogMessageToChannelAsync($"We had a new subscriber! Thank you for subbing {e.Subscriber.DisplayName}.", "Announcements"));
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            _logger.LogInformation($"Received message. {e.ChatMessage.Message}");
            Task.Run(async () => await _discordChatService.LogMessageToChannelAsync($"Received message. {e.ChatMessage.Message}", "TestBotChannel"));
        }

        private void OnServiceStarted(object sender, OnServiceStartedArgs e)
        {
            _logger.LogInformation($"Connected to service.");
        }

        private void OnServiceTick(object sender, OnServiceTickArgs e)
        {
            _logger.LogInformation($"Service ticked.");
        }

        private void OnNewFollowersDetected(object sender, OnNewFollowersDetectedArgs e)
        {
            var databaseUsers = _database.GetCollection<TwitchUser>();
            var newFollowers = new List<string>();
            foreach (var newUser in e.NewFollowers)
            {
                if (databaseUsers.FindOne(u => u.Id == newUser.FromUserId) == null)
                    newFollowers.Add(newUser.FromUserId);
            }

            if (newFollowers.Count() > 0)
            {
                var users = _twitchAPI.Helix.Users.GetUsersAsync(newFollowers).Result;
                Array.ForEach(users.Users, u => _database.GetCollection<TwitchUser>().Insert(new TwitchUser { Id = u.Id, DisplayName = u.DisplayName }));

                if (users.Users.Count() > 0)
                {
                    var followersNames = string.Join(", ", users.Users.Select(u => u.DisplayName).ToList());
                    _logger.LogInformation($"We had a new follower.  Thank you for following { followersNames }");
                    _client.SendMessage(e.Channel, $"We had a new follower.  Thank you for following { followersNames }");
                    Task.Run(async () => await _discordChatService.LogMessageToChannelAsync($"We had a new follower.  Thank you for following { followersNames }", "Announcements"));
                }
            }
        }
    }
}
