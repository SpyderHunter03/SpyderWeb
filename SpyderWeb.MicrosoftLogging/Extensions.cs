using Discord;
using Discord.Rest;
using Microsoft.Extensions.Logging;
using System;

namespace SpyderWeb.MicrosoftLogging
{
    public static class Extensions
    {
        public static void UseMicrosoftLogging(this BaseDiscordClient client, ILogger logger, Func<LogMessage, Exception, string> formatter = null)
        {
            var adaptor = new LogAdapter(logger, formatter);
            client.Log += adaptor.Log;
        }
    }
}
