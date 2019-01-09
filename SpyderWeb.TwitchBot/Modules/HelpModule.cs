using Discord;
using Discord.Commands;
using SpyderWeb.EmojiTools;
using System.Text;
using System.Threading.Tasks;

namespace SpyderWeb.Discord.Modules
{
    [Name("Help")]
    public class HelpModule : SpyderModuleBase
    {
        private readonly CommandService _commands;
        //private readonly IDiscordCustomCommandService _discordCustomCommandService;

        public HelpModule(
            CommandService commands, 
            //DiscordCustomCommandService discordCustomCommandService, 
            IEmojiService emojiService) : base(emojiService)
        {
            _commands = commands;
            //_discordCustomCommandService = discordCustomCommandService;
        }

        [Command("help")]
        [Name("help")]
        [Summary("Get bot help")]
        public async Task HelpAsync()
        {
            var content = new StringBuilder();
            content.AppendLine(Format.Bold("Spyder"));
            content.AppendLine("A utility bot for Discord.Net\n");
            content.AppendLine("```");

            foreach (var module in _commands.Modules)
            {
                //if (_discordCustomCommandService.TagsModule != null && module == _discordCustomCommandService.TagsModule) continue;
                if (module.Commands.Count == 0) continue;

                content.AppendLine(module.Name);

                foreach (var command in module.Commands)
                    content.AppendLine(command.Name.PadRight(27) + command.Summary);

                content.AppendLine();
            }

            content.AppendLine("```");
            content.AppendLine("Commands suffixed with a '*' are restricted to elevated actors");
            await ReplyAsync(content.ToString());
        }
    }
}
