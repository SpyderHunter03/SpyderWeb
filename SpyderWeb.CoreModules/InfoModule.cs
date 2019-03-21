using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SpyderWeb.CoreModules.Models;
using SpyderWeb.Events;
using SpyderWeb.Events.EventArguments;

namespace SpyderWeb.CoreModules
{
    public static class InfoModule // : CoreModule<IInfoModule>
    {
        public static void MessageReceived(object sender, MessageReceivedArgs args)
        {
            if (args.Message.IndexOf($"{args.Prefix}info", StringComparison.InvariantCultureIgnoreCase) == 0)
                InfoMethod(sender, args);
        }

        private static void InfoMethod(object sender, MessageReceivedArgs args)
        {
            var responseMessage = $"SpyderBot is a private-use Discord/Twitch bot.\n\n"
                + $"**Info**\n"
                + $"- Runtime: {RuntimeInformation.FrameworkDescription} {RuntimeInformation.ProcessArchitecture} "
                + $"({RuntimeInformation.OSDescription} {RuntimeInformation.OSArchitecture})\n"
                + $"- Uptime: {GetUptime()}\n\n"
                + $"- Heap Size: {GetHeapSize()}MiB\n";
                if (args.Sender == (int)MessageSender.Discord && args.MessageContext as DiscordContext != null)
                {
                    var discordContext = args.MessageContext as DiscordContext;
                    responseMessage += $"\n**Stats**\n"
                    + $"- Library: Discord.Net ({discordContext.Version})\n"
                    + $"- Bot Author: {discordContext.AppOwner.Name} ({discordContext.AppOwner.Id})\n"
                    + $"- Originating Guild: {discordContext.Guild.Name} ({discordContext.Guild.Id})\n"
                    + $"- Originating Channel: {discordContext.Channel.Name} ({discordContext.Channel.Id})\n"
                    + $"- Guilds Using Bot: {discordContext.Guilds.Count()}\n"
                    + $"- Channels In Using Guilds: {discordContext.Guilds.Sum(g => g.Channels.Count())}\n"
                    + $"- Users In Using Guilds: {discordContext.Guilds.Sum(g => g.Users.Count())}";
                }

            SendMessage(sender, responseMessage);
        }

        private static void SendMessage(object sender, string message)
        {
            EventPublisher.OnSendMessageEvent(sender, new SendMessageArgs(message, sender));
        }

        private static string GetUptime() => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\ss");
        private static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();
    }
}
