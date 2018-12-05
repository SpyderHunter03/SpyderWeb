using System;
using Discord;

namespace SpyderWeb.Data.Tags
{
    public class Tag
    {
        public Tag() { }

        public Tag(string name, string content, IUser owner)
        {
            Name = name;
            Content = content;
            OwnerId = owner.Id;

            CreatedAt = DateTimeOffset.Now;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public ulong OwnerId { get; set; }
        
        public uint Color { get; set; }
    }
}
