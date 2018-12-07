using SpyderWeb.Data.Tags;
using System.Collections.Generic;

namespace SpyderWeb.Services
{
    public interface IDatabaseService
    {
        IEnumerable<Tag> GetTags();
        bool AddTag(Tag tag);
        bool UpdateTag(Tag tag);
        bool DeleteTag(Tag tag);
    }
}