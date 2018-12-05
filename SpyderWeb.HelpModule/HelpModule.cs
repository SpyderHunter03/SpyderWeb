using Discord;
using Discord.Commands;
using SpyderWeb.Modules;
using System.Text;
using System.Threading.Tasks;

namespace SpyderWeb.HelpModule
{
    [Name("Help")]
    public class HelpModule : SpyderModuleBase
    {
        public CommandService CommandService { get; set; }
        //public TagService TagService { get; set; }

        [Command("help")]
        [Name("help")]
        [Summary("Get bot help")]
        public async Task HelpAsync()
        {
            var content = new StringBuilder();
            content.AppendLine(Format.Bold("Spyder"));
            content.AppendLine("A utility bot for Discord.Net\n");
            content.AppendLine("```");

            foreach (var module in CommandService.Modules)
            {
                //if (TagService.Module.IsSpecified && module == TagService.Module.Value) continue;
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
