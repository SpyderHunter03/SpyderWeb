using Discord;
using System;
using System.Linq;

namespace SpyderWeb.Options
{
    public class DiscordFilter
    {
        public ulong[] Guilds { get; set; } = new ulong[0];
        public Channel[] Channels { get; set; } = new Channel[0];
        public ulong[] Users { get; set; } = new ulong[0];

        public bool IsWhitelisted(IChannel channel) 
            => Channels.Select(c => c.Id).Contains(channel.Id) || Guilds.Contains((channel as IGuildChannel)?.GuildId ?? 1UL);
        public bool IsElevated(IUser user) => Users.Contains(user.Id);
    }

    public class Channel
    {
        public ulong Id { get; set; } = new ulong();
        public string Name { get; set; } = "";
    }
}
