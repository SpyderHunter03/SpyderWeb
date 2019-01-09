using Discord.Commands;
using Microsoft.Extensions.Logging;
using SpyderWeb.Database;
using SpyderWeb.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SpyderWeb.TagService
{
    public class TagService : ITagService
    {
        private readonly CommandService _commands;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDatabaseService<Tag> _database;
        private readonly ILogger _logger;

        public ModuleInfo Module { get; private set; }

        public TagService(
            CommandService commands,
            IServiceProvider serviceProvider,
            IDatabaseService<Tag> database,
            ILoggerFactory loggerFactory)
        {
            _commands = commands;
            _serviceProvider = serviceProvider;
            _database = database;
            _logger = loggerFactory.CreateLogger("tags");

            Init();
        }

        private async void Init()
        {
            var all =AppDomain.CurrentDomain.GetAssemblies()
                        .Where(assembly => assembly.FullName.StartsWith("SpyderWeb"))
                        //.Select(Assembly.Load)
                        .SelectMany(x => x.DefinedTypes)
                        //.Where(type => type.IsClass && !type.IsAbstract)
                        .Where(type => type.IsClass && !type.IsAbstract && typeof(SpyderModuleBase).IsSubclassOf(type))
                        //.Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(SpyderModuleBase)))
                        //.Where(type => type.IsSubclassOf(typeof(SpyderModuleBase)))
                        //.Where(type => type.IsAssignableFrom(typeof(SpyderModuleBase)))
                        //.Where(type => typeof(SpyderModuleBase).GetTypeInfo().IsAssignableFrom(type.AsType()));
                        .ToList();
                        
            foreach (var type in all)
            {
                await _commands.AddModuleAsync(type, _serviceProvider);
            }
            //var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.ManifestModule.Name.StartsWith("SpyderWeb"));
            //Add all modules in the solution
            //foreach (Assembly a in assemblies)
            //{
            //    if (a.GetTypes().Any(t => t.IsSubclassOf(typeof(SpyderModuleBase))))
            //        await _commands.AddModulesAsync(a, _serviceProvider);
            //}

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
