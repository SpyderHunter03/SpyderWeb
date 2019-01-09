using Discord;
using Discord.Commands;
using SpyderWeb.EmojiTools;
using SpyderWeb.Models;
using SpyderWeb.Overwatch;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SpyderWeb.Discord.Modules
{
    public class OverwatchModule : SpyderModuleBase
    {
        private readonly IOverwatchService _overwatchService;
        private readonly string _privateProfileStatString;

        public OverwatchModule(
            IOverwatchService overwatchService,
            IEmojiService emojiService) : base(emojiService)
        {
            _overwatchService = overwatchService;
            _privateProfileStatString = _emojiService.GetEmojiFromText("closed_lock_with_key").Name;
        }

        [Command("overwatch sr")]
        [Name("overwatch sr <name>")]
        [Alias("ow sr", "ow")]
        [Summary("Get SR for Overwatch Character")]
        public async Task OverwatchSRAsync(string name)
        {
            var player = await _overwatchService.GetOverwatchPlayerModel(name);
            if (player == null)
            {
                await ReplyAsync($"Player {name} was not found.  Please check your numbers and CAPITALIZATION MATTERS!");
            }
            else
            {
                var embed = new EmbedBuilder()
                    .WithDescription($"Overwatch.Net for: {player.Username}")
                    .WithUrl(player.ProfileUrl)
                    .WithColor(16744960)
                    .WithFooter(f =>
                    {
                        f.IconUrl = "https://steamusercontent-a.akamaihd.net/ugc/767148359772513521/87347549402E0F321074CFE9DA98C24212AF3DD3/?interpolation=lanczos-none&output-format=jpeg&output-quality=95&fit=inside%7C637%3A358&composite-to=*,*%7C637%3A358&background-color=black";
                        f.Text = "Powered by Overwatch.Net";
                    })
                    .WithImageUrl(player.IsProfilePrivate || string.IsNullOrWhiteSpace(player.CompetitiveRankImageUrl) ? player.PlayerLevelImage : player.CompetitiveRankImageUrl)
                    .WithThumbnailUrl(player.ProfilePortraitUrl)
                    .WithAuthor(a =>
                    {
                        a.Name = $"{name}'s Stats";
                        a.Url = player.ProfileUrl;
                    })
                    .AddField("Level", player.PlayerLevel, true)
                    .AddField("SR", CheckForPrivate(player, () => player.CompetitiveRank.ToString()), true)
                    .AddField("Career Competitive K.D.", CheckForPrivate(player, () => player.CompetitiveStats.FirstOrDefault(cs => cs.CategoryName.Equals("Something", StringComparison.InvariantCultureIgnoreCase))?.Value.ToString() ?? "N/A"), true)
                    .AddField("Career Win Percentage", CheckForPrivate(player, () => player.CompetitiveStats.FirstOrDefault(cs => cs.CategoryName.Equals("Something", StringComparison.InvariantCultureIgnoreCase))?.Value.ToString()) ?? "N/A", true);
                    
                await ReplyAsync(embed: embed.Build());
            }
        }

        private string CheckForPrivate(OverwatchPlayerModel player, Func<string> MethodToRun)
        {
            if (player.IsProfilePrivate)
                return _privateProfileStatString;
            return MethodToRun();
        }
    }
}