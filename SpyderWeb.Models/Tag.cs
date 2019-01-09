using System;

namespace SpyderWeb.Models
{
    public class Tag : IDatabaseObject
    {
        public Tag() { }

        public Tag(string name, string content, ulong ownerId)
        {
            Name = name;
            Content = content;
            OwnerId = ownerId;

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
