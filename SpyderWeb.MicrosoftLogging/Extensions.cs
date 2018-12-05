using System;
using Microsoft.Extensions.Logging;
using Discord.WebSocket;
using Discord;

namespace SpyderWeb.MicrosoftLogging
{
    public static class Extensions
    {
        public static void UseMicrosoftLogging(this DiscordSocketClient client, ILogger logger, Func<LogMessage, Exception, string> formatter = null)
        {
            var adaptor = new LogAdapter(logger, formatter);
            client.Log += adaptor.Log;
        }
    }
}
