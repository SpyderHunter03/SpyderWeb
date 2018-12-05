using Discord;
using Discord.Commands;
using LiteDB;
using Microsoft.Extensions.Logging;
using SpyderWeb.Data.Tags;
using System.Linq;
using System.Threading.Tasks;

namespace SpyderWeb.Services
{
    public class TagService
    {
        private readonly CommandService _commands;
        private readonly LiteDatabase _database;
        private readonly ILogger _logger;

        public Optional<ModuleInfo> Module => module;
        private Optional<ModuleInfo> module;

        public TagService(CommandService commands, LiteDatabase database, ILoggerFactory loggerFactory)
        {
            _commands = commands;
            _database = database;
            _logger = loggerFactory.CreateLogger("tags");

            module = new Optional<ModuleInfo>();
        }

        public async Task BuildTagsAsync()
        {
            if (module.IsSpecified)
                await _commands.RemoveModuleAsync(module.Value);

            var tags = _database.GetCollection<Tag>().FindAll();

            module = await _commands.CreateModuleAsync("", module =>
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
