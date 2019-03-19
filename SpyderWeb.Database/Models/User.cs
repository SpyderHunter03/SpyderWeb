using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpyderWeb.Database.Models
{
    public class User : BaseDatabaseObject
    {
        public ulong DiscordId { get; set; }
        public string DiscordName { get; set; }
        public ulong TwitchId { get; set; }
        public string TwitchName { get; set; }
        public ulong FacebookId { get; set; }
        public string FacebookName { get; set; }
        public ulong TwitterId { get; set; }
        public string TwitterName { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}