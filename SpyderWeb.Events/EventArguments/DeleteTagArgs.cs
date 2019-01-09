using SpyderWeb.Models;
using System;

namespace SpyderWeb.Events.EventArguments
{
    public class DeleteTagArgs : EventArgs
    {
        public Tag CreatedTag { get; set; }

        public DeleteTagArgs(Tag tag)
        {
            CreatedTag = tag;
        }
    }
}
