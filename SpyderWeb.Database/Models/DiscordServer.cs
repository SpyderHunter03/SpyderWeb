using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpyderWeb.Database.Models
{
    public class DiscordServer : BaseDatabaseObject
    {
        public ulong ServerId { get; set; }
        public string ServerName { get; set; }
        public int OwnerId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}