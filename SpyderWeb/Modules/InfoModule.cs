using Discord;
using Discord.Commands;
using Microsoft.Extensions.Options;
using SpyderWeb.EmojiTools;
using SpyderWeb.Options;
using SpyderWeb.Preconditions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SpyderWeb.Modules
{
    [Name("Bot Information")]
    public class InfoModule : SpyderModuleBase
    {
        public InfoModule(IEmojiService emojiService, IOptions<DiscordFilter> filter) : base (emojiService) => Filter = filter.Value;

        [DontInject]
        public DiscordFilter Filter { get; set; }

        [Command("info")]
        [Alias("about","whoami","owner")]
        [Name("info")]
        [Summary("Get bot info")]
        public async Task InfoAsync()
        {
            var app = await Context.Client.GetApplicationInfoAsync();
            var channel = Context.Channel;
            var guild = Context.Guild;

            await ReplyAsync(
                $"SpyderWeb is a private-use Discord bot for Discord.Net's support channels.\n\n" +
                $"{Format.Bold("Info")}\n" +
                $"- Author: {app.Owner} ({app.Owner.Id})\n" +
                $"- Channel: {channel.Name} ({channel.Id})\n" +
                $"- Guild: {guild.Name} ({guild.Id})\n" +
                $"- Library: Discord.Net ({DiscordConfig.Version})\n" +
                $"- Runtime: {RuntimeInformation.FrameworkDescription} {RuntimeInformation.ProcessArchitecture} " +
                    $"({RuntimeInformation.OSDescription} {RuntimeInformation.OSArchitecture})\n" +
                $"- Uptime: {GetUptime()}\n\n" +

                $"{Format.Bold("Stats")}\n" +
                $"- Heap Size: {GetHeapSize()}MiB\n" +
                $"- Guilds: {Context.Client.Guilds.Count}\n" +
                $"- Channels: {Context.Client.Guilds.Sum(g => g.Channels.Count)}\n" +
                $"- Users: {Context.Client.Guilds.Sum(g => g.Users.Count)}\n");
        }

        [Command("debug")]
        [RequireElevatedUser]
        [Name("debug*")]
        [Summary("Get bot debug info")]
        public async Task DebugAsync()
        {
            var embed = new EmbedBuilder()
                .WithTitle("Debug")
                .AddField("Whitelisted Channels",
                    string.Join(", ", Filter.Channels.Select(x => MentionUtils.MentionChannel(x.Id))))
                .AddField("Elevated Users",
                    string.Join(", ", Filter.Users.Select(x => MentionUtils.MentionUser(x))))
                .Build();

            await ReplyAsync("", embed: embed);
        }

        private static string GetUptime() => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\ss");
        private static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();
    }
}
