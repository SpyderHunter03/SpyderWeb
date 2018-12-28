using Discord;
using Discord.Commands;
using OverwatchAPI;
using SpyderWeb.EmojiTools;
using SpyderWeb.ModuleBase;
using System.Threading.Tasks;

namespace SpyderWeb.OverwatchModule
{
    public class OverwatchModule : SpyderModuleBase
    {
        public OverwatchModule(IEmojiService emojiService) : base(emojiService) { }

        [Command("overwatch sr")]
        [Name("overwatch sr <name>")]
        [Alias("ow sr", "ow")]
        [Summary("Get SR for Overwatch Character")]
        public async Task OverwatchSRAsync(string name)
        {
            using (var owClient = new OverwatchClient())
            {
                Player player = await owClient.GetPlayerAsync(name);

                if (player == null)
                {
                    await ReplyAsync($"Player {name} was not found.  Please check your numbers and CAPITALIZATION MATTERS!");
                }
                else
                {
                    var embed = new EmbedBuilder()
                        .WithTitle(name)
                        .WithUrl(player.ProfileUrl);
                    if (player.IsProfilePrivate)
                    {
                        embed
                            .AddField("Error", "This players profile is private.");
                    }
                    else
                    {
                        embed
                            .AddField("Current SR", player.CompetitiveRank)
                            .WithThumbnailUrl(player.CompetitiveRankImageUrl ?? player.ProfilePortraitUrl);
                    }

                    await ReplyAsync("", embed: embed.Build());
                }
            }
        }
    }
}
