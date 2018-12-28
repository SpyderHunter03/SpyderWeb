using Microsoft.Extensions.Logging;
using System;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;
using TwitchLib.PubSub.Interfaces;

namespace SpyderWeb.Twitch
{
    public class TwitchPubSubClient
    {
        private readonly ITwitchPubSub _client;
        private readonly ILogger<TwitchPubSub> _logger;
        private readonly string _twitchChannelId;

        public TwitchPubSubClient(
            string twitchChannelId,
            ITwitchPubSub twitchPubSub,
            ILoggerFactory loggerFactory)
        {
            _twitchChannelId = twitchChannelId;
            _logger = loggerFactory.CreateLogger<TwitchPubSub>();
            if (_client == null)
            {
                _client = new TwitchPubSub(_logger);
                Init();
            }
        }

        public void Init()
        {
            _client.ListenToSubscriptions(_twitchChannelId);
            _client.OnPubSubServiceConnected += OnPubSubServiceConnected;
            _client.Connect();
        }

        private void OnPubSubServiceConnected(object sender, EventArgs e)
        {
            _client.SendTopics();
        }

        private void OnListenResponse(object sender, OnListenResponseArgs e)
        {
            if (!e.Successful)
                throw new Exception($"Failed to listen!  Response: {e.Response}");
        }

        private void OnStreamUp(object sender, OnStreamUpArgs e)
        {
            _logger.LogInformation($"Stream just went up! Play delay: {e.PlayDelay}, server time: {e.ServerTime}");
        }

        private void OnStreamDown(object sender, OnStreamDownArgs e)
        {
            _logger.LogInformation($"Stream just went down! Server time: {e.ServerTime}");
        }
    }
}
