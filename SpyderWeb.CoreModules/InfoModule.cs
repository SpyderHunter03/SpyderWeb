using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SpyderWeb.Events;
using SpyderWeb.Events.EventArguments;

namespace SpyderWeb.CoreModules
{
    public static class InfoModule // : CoreModule<IInfoModule>
    {
        public static void MessageReceived(object sender, MessageReceivedArgs args)
        {
            if (args.Message.IndexOf("!info", StringComparison.InvariantCultureIgnoreCase) >= 0)
                InfoMethod(sender, args);
        }

        private static void InfoMethod(object sender, MessageReceivedArgs args)
        {
            var responseMessage = $"SpyderBot is a private-use Discord/Twitch bot.\n\n"
                + $"**Info**\n"
                //+ $"- Author: {app.Owner} ({app.Owner.Id})\n"
                //+ $"- Channel: {channel.Name} ({channel.Id})\n"
                //+ $"- Guild: {guild.Name} ({guild.Id})\n"
                //+ $"- Library: Discord.Net ({DiscordConfig.Version})\n"
                //+ $"- Runtime: {RuntimeInformation.FrameworkDescription} {RuntimeInformation.ProcessArchitecture} "
                //+    $"({RuntimeInformation.OSDescription} {RuntimeInformation.OSArchitecture})\n"
                + $"- Uptime: {GetUptime()}\n\n"

                + $"**Stats**\n"
                + $"- Heap Size: {GetHeapSize()}MiB\n"
                //+ $"- Guilds: {Context.Client.Guilds.Count}\n"
                //+ $"- Channels: {Context.Client.Guilds.Sum(g => g.Channels.Count)}\n"
                //+ $"- Users: {Context.Client.Guilds.Sum(g => g.Users.Count)}\n"
                ;

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
