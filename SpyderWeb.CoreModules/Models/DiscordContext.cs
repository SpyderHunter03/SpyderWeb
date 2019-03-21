using System.Collections.Generic;

namespace SpyderWeb.CoreModules.Models
{
    public class DiscordContext
    {
        public string Version { get; set; }
        public DiscordObject AppOwner { get; set; }
        public DiscordObject Channel { get; set; }
        public DiscordObject Guild { get; set; }
        public IEnumerable<DiscordStats> Guilds { get; set; }

        public class DiscordObject {
            public ulong Id { get; set; }
            public string Name { get; set; }
        }

        public class DiscordStats {
            public IEnumerable<DiscordObject> Channels { get; set; }
            public IEnumerable<DiscordObject> Users { get; set; }
        }
    }
}