using Discord;
using Discord.Commands;
using Microsoft.Extensions.Options;
using SpyderWeb.Configurations;
using SpyderWeb.Database;
using SpyderWeb.Discord.Modules.Preconditions;
using SpyderWeb.EmojiTools;
using SpyderWeb.Events;
using SpyderWeb.Events.EventArguments;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SpyderWeb.Discord.Modules
{
    [Name("Tag Management")]
    public class TagModule : SpyderModuleBase
    {
        // private readonly Credentials _options;
        // private readonly IDatabaseService<Tag> _database;

        public TagModule(
            IEmojiService emoji
            // , IOptionsMonitor<Credentials> options
            // , IDatabaseService<Tag> database
            ) 
        : base(emoji)
        {
            // _options = options.CurrentValue;
            // _database = database;
        }

        // [Command("tag create")]
        // [RequireElevatedUser]
        // [Name("tag create* <name> <text>")]
        // [Summary("Add a tag")]
        // public async Task CreateTagAsync(string name, [Remainder] string content)
        // {
        //     var tag = new Tag(name, content, Context.User.Id);
        //     _database.Add(tag);

        //     EventPublisher.OnCreateTagEvent(this, new CreateTagArgs(tag));

        //     await ReactAsync(Pass);
        // }

        // [Command("tag set name")]
        // [Alias("tag setname", "tag update name", "tag name")]
        // [RequireElevatedUser]
        // [Name("tag name* <name> <new>")]
        // [Summary("Change a tag's name")]
        // public async Task SetNameAsync(string before, string after)
        // {
        //     var tags = _database.GetAll();
        //     var oldTag = tags.FirstOrDefault(x => x.Name == before);
        //     if (oldTag == null)
        //     {
        //         await ReactAsync(TagNotFound);
        //         return;
        //     }

        //     var newTag = oldTag;
        //     newTag.Name = after;
        //     newTag.ActorId = Context.User.Id;
        //     newTag.UpdatedAt = DateTimeOffset.Now;

        //     _database.Update(newTag);

        //     EventPublisher.OnUpdateTagEvent(this, new UpdateTagArgs(oldTag, newTag));
            
        //     await ReactAsync(Pass);
        // }

        // [Command("tag set content")]
        // [Alias("tag set text", "tag update text", "tag text", "tag content")]
        // [RequireElevatedUser]
        // [Name("tag text* <name> <text>")]
        // public async Task SetContentAsync(string name, [Remainder] string content)
        // {
        //     var tags = _database.GetAll();
        //     var oldTag = tags.FirstOrDefault(x => x.Name == name);
        //     if (oldTag == null)
        //     {
        //         await ReactAsync(TagNotFound);
        //         return;
        //     }

        //     var newTag = oldTag;
        //     newTag.Content = content;
        //     newTag.ActorId = Context.User.Id;
        //     newTag.UpdatedAt = DateTimeOffset.Now;

        //     _database.Update(newTag);

        //     EventPublisher.OnUpdateTagEvent(this, new UpdateTagArgs(oldTag, newTag));

        //     await ReactAsync(Pass);
        // }

        // [Command("tag get content")]
        // [Alias("tag raw", "tag content")]
        // [Name("tag raw <name>")]
        // [Summary("Get a tag's raw content")]
        // public async Task GetRawContentAsync(string name)
        // {
        //     var tags = _database.GetAll();
        //     var tag = tags.FirstOrDefault(x => x.Name == name);
        //     if (tag == null)
        //     {
        //         await ReactAsync(TagNotFound);
        //         return;
        //     }

        //     await ReplyAsync(Format.Sanitize(tag.Content));
        // }

        // [Command("tag delete")]
        // [RequireElevatedUser]
        // [Name("tag delete* <name>")]
        // [Summary("Pretend to delete a tag")]
        // public Task DeleteAsync([Remainder] string name)
        //     => ReplyAsync($"Are you sure you want to do this? If so, use {_options.DiscordPrefix}tag destroy {name}");

        // [Command("tag destroy")]
        // [RequireElevatedUser]
        // [Name("tag destroy* <name>")]
        // [Summary("Actually delete a tag")]
        // public async Task DestroyAsync(string name)
        // {
        //     var tags = _database.GetAll();
        //     var tag = tags.FirstOrDefault(x => x.Name == name);
        //     if (tag == null)
        //     {
        //         await ReactAsync(TagNotFound);
        //         return;
        //     }

        //     _database.Delete(tag);

        //     EventPublisher.OnDeleteTagEvent(this, new DeleteTagArgs(tag));
            
        //     await ReactAsync(Removed);
        // }

        // [Command("tag list")]
        // [Alias("tags")]
        // [Name("tags")]
        // [Summary("List the tags")]
        // public async Task ListAsync()
        // {
        //     var tags = _database.GetAll();
        //     await ReplyAsync($"Tags: {string.Join(", ", tags.Select(t => t.Name))}");
        // }

        // [Command("tag info")]
        // [Alias("tag owner", "tag whois", "tag about")]
        // [Name("tag info <name>")]
        // [Summary("Get info about a tag")]
        // public async Task InfoAsync(string name)
        // {
        //     var tags = _database.GetAll();
        //     var tag = tags.FirstOrDefault(x => x.Name == name);
        //     if (tag == null)
        //     {
        //         await ReactAsync(TagNotFound);
        //         return;
        //     }
        //     var modifiedBy = tag.ActorId.HasValue ? $"{Context.Guild.GetUser(tag.ActorId.Value)?.ToString() ?? "<user unknown>"}" : "<not modified>";
        //     await ReplyAsync(
        //         $"{Format.Bold("Tag Info")}\n" +
        //         $"- Name: {tag.Name}\n" +
        //         $"- Owner: {Context.Guild.GetUser(tag.OwnerId)?.ToString() ?? "<user unknown>"} (ID: {tag.OwnerId})\n" +
        //         $"- Created At: {tag.CreatedAt}\n" +
        //         $"- Modifed By: {(tag.ActorId.HasValue ? $"{Context.Guild.GetUser(tag.ActorId.Value)?.ToString() ?? "<user unknown>"}" : "<not modified>")}\n" +
        //         $"- Modified At: {(tag.UpdatedAt.HasValue ? tag.UpdatedAt.ToString() : "<not modified>")}\n"
        //         );
        // }
    }
}
