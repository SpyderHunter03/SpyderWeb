using Discord;
using Discord.Commands;
using OverwatchAPI;
using System.Threading.Tasks;

namespace SpyderWeb.Modules
{
    public class OverwatchModule : SpyderModuleBase
    {
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
                        .AddField("Current SR", player.CompetitiveRank)
                        .WithUrl(player.ProfileUrl)
                        .WithThumbnailUrl(player.CompetitiveRankImageUrl ?? player.ProfilePortraitUrl);

                    await ReplyAsync("", embed: embed.Build());
                }
            }
        }
    }
}
