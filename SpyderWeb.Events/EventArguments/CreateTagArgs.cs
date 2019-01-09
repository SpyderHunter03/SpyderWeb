using SpyderWeb.Models;
using System;

namespace SpyderWeb.Events.EventArguments
{
    public class CreateTagArgs : EventArgs
    {
        public Tag CreatedTag { get; set; }

        public CreateTagArgs(Tag tag)
        {
            CreatedTag = tag;
        }
    }
}
