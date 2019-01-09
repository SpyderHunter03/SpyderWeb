using Discord;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SpyderWeb.Discord
{
    public class DiscordLogAdapter
    {
        private ILogger _logger;
        private readonly Func<LogMessage, Exception, string> _formatter;

        public DiscordLogAdapter(ILogger logger, Func<LogMessage, Exception, string> formatter = null)
        {
            _logger = logger;
            _formatter = formatter ?? DefaultFormatter;
        }

        public Task Log(LogMessage message)
        {
            _logger.Log(GetLogLevel(message.Severity), default(EventId), message, message.Exception, _formatter);
            return Task.Delay(0);
        }

        private string DefaultFormatter(LogMessage message, Exception _) => message.ToString();

        private static LogLevel GetLogLevel(LogSeverity severity) => (LogLevel)(Math.Abs((int)severity - 5));
    }
}
