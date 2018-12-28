using Discord;
using Discord.Commands;
using SpyderWeb.EmojiTools;
using SpyderWeb.ModuleBase;
using SpyderWeb.TagService;
using System.Text;
using System.Threading.Tasks;

namespace SpyderWeb.HelpModule
{
    [Name("Help")]
    public class HelpModule : SpyderModuleBase
    {
        private readonly CommandService _commandService;
        private readonly ITagService _tagService;

        public HelpModule(
            IEmojiService emojiService,
            CommandService commandService,
            ITagService tagService) : base(emojiService)
        {
            _commandService = commandService;
            _tagService = tagService;
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

            foreach (var module in _commandService.Modules)
            {
                if (_tagService.Module != null && module == _tagService.Module) continue;
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
