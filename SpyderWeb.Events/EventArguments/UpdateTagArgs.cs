using SpyderWeb.Models;
using System;

namespace SpyderWeb.Events.EventArguments
{
    public class UpdateTagArgs : EventArgs
    {
        public Tag BeforeTag { get; set; }
        public Tag AfterTag { get; set; }

        public UpdateTagArgs(Tag beforeTag, Tag afterTag)
        {
            BeforeTag = beforeTag;
            AfterTag = afterTag;
        }
    }
}
