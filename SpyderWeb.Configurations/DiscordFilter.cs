using Discord;
using System;
using System.Linq;

namespace SpyderWeb.Configurations
{
    public class DiscordFilter
    {
        public DiscordGuildFilter[] Guilds { get; set; } = new DiscordGuildFilter[0];
        public DiscordUserFilter[] Users { get; set; } = new DiscordUserFilter[0];

        /* I don't think I want this because the bot is already Whitelisted by the guild having
            it in their server.  And I want the channel whitelisting to be module based. */
        // public bool IsWhitelisted(IChannel channel)
        //     =>  Guilds.Select(g => g.Id).Contains((channel as IGuildChannel)?.GuildId ?? 1UL)
        //         && Guilds.SelectMany(g => g.Channels).Select(c => c.Id).Contains(channel.Id);
        public bool IsElevated(IUser user) => Users.Select(u => u.Id).Contains(user.Id);
    }

    public class DiscordGuildFilter
    {
        public ulong Id { get; set; } = new ulong();
        public string Name { get; set; } = "";

        public DiscordChannelFilter[] Channels { get; set; } = new DiscordChannelFilter[] {};
    }

    public class DiscordChannelFilter
    {
        public ulong Id { get; set; } = new ulong();
        public string Name { get; set; } = "";
    }

    public class DiscordUserFilter
    {
        public ulong Id { get; set; } = new ulong();
        public string Name { get; set; } = "";
    }
}
