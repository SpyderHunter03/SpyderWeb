using Discord;
using System;

namespace SpyderWeb.Models
{
    public class Tag : IDatabaseObject
    {
        public Tag() { }

        public Tag(string name, string content, IUser owner)
        {
            Name = name;
            Content = content;
            OwnerId = owner.Id;

            CreatedAt = DateTimeOffset.Now;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public ulong OwnerId { get; set; }
        public ulong? ActorId { get; set; }
    }
}
