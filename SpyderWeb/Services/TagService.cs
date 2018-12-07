using Discord.Commands;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace SpyderWeb.Services
{
    public class TagService : ITagService
    {
        private readonly CommandService _commands;
        private readonly IDatabaseService _database;
        private readonly ILogger _logger;

        public ModuleInfo Module { get; private set; }

        public TagService(
            CommandService commands,
            IDatabaseService database,
            ILoggerFactory loggerFactory)
        {
            _commands = commands;
            _database = database;
            _logger = loggerFactory.CreateLogger("tags");

            Init();
        }

        private async void Init()
        {
            await BuildTagsAsync();
        }

        public async Task BuildTagsAsync()
        {
            if (Module == null)
                await _commands.RemoveModuleAsync(Module);

            var tags = _database.GetTags();

            Module = await _commands.CreateModuleAsync("", module =>
            {
                foreach (var tag in tags)
                {
                    module.AddCommand(tag.Name, (context, @params, provider, command) =>
                    {
                        return context.Channel.SendMessageAsync(
                            $"{tag.Name}: {tag.Content}");
                    },
                    command => { });
                }
            });

            _logger.LogInformation("Build {} tags successfully.", tags.Count());
        }
    }
}
