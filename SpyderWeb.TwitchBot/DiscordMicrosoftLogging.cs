using Discord;
using Discord.Rest;
using Microsoft.Extensions.Logging;
using System;

namespace SpyderWeb.Discord
{
    public static class DiscordMicrosoftLogging
    {
        public static void UseMicrosoftLogging(this BaseDiscordClient client, ILogger logger, Func<LogMessage, Exception, string> formatter = null)
        {
            var adaptor = new DiscordLogAdapter(logger, formatter);
            client.Log += adaptor.Log;
        }
    }
}
