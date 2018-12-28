using Discord.Commands;
using Microsoft.Extensions.Logging;
using SpyderWeb.Database;
using SpyderWeb.Models;
using SpyderWeb.ModuleBase;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SpyderWeb.TagService
{
    public class TagService : ITagService
    {
        private readonly CommandService _commands;
        private readonly IDatabaseService<Tag> _database;
        private readonly ILogger _logger;

        public ModuleInfo Module { get; private set; }

        public TagService(
            CommandService commands,
            IDatabaseService<Tag> database,
            ILoggerFactory loggerFactory)
        {
            _commands = commands;
            _database = database;
            _logger = loggerFactory.CreateLogger("tags");

            Init();
        }

        private async void Init()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.ManifestModule.Name.StartsWith("SpyderWeb"));
            //Add all modules in the solution
            foreach (Assembly a in assemblies)
            {
                if (a.GetTypes().Any(t => t.IsSubclassOf(typeof(SpyderModuleBase))))
                    await _commands.AddModulesAsync(a);
            }

            await BuildTagsAsync();
        }

        public async Task BuildTagsAsync()
        {
            if (Module == null)
                await _commands.RemoveModuleAsync(Module);

            var tags = _database.GetAll();

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

            _logger.LogInformation($"Build {tags.Count()} tags successfully.");
        }
    }
}
