using System;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace SpyderWeb.Twitch
{
    public class Bot
    {
        private TwitchClient _client;
        private ConnectionCredentials _credentials;

        public Bot(string userName, string oauthToken)
        {
            _credentials = new ConnectionCredentials(userName, oauthToken);
        }

        public async Task InitializeAsync()
        {
            _client = new TwitchClient();
            _client.Initialize(_credentials, "SpyderHunter03");

            _client.OnConnected += OnConnected;
            _client.OnNewSubscriber += OnNewSubscriber;

            await Task.Run(() => _client.Connect());
        }

        private void OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");
        }

        private void OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            Console.WriteLine($"We had a new subscriber. Username: {e.Subscriber.DisplayName}");
            _client.SendMessage(e.Channel, $"We had a new subscriber. Username: {e.Subscriber.DisplayName}");
        }
    }
}
