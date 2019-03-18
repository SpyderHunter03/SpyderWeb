using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Discord.Commands;
using Discord.WebSocket;
using SpyderWeb.Configurations;

namespace SpyderWeb.Discord.Modules.Preconditions
{
    public class RequireElevatedUser : PreconditionAttribute
    {
        private static readonly Task<PreconditionResult> NotUser = Task.FromResult(PreconditionResult.FromError("This command may only be ran in a guild."));
        private static readonly Task<PreconditionResult> NotElevated = Task.FromResult(PreconditionResult.FromError("You are not elevated in this guild."));
        private static readonly Task<PreconditionResult> Elevated = Task.FromResult(PreconditionResult.FromSuccess());

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var filter = services.GetRequiredService<IOptionsMonitor<DiscordFilter>>().CurrentValue;
            if (!(context.User is SocketGuildUser user)) return NotUser;
            if (filter.IsElevated(context.User as SocketGuildUser)) return Elevated;
            return NotElevated;
        }
    }
}
