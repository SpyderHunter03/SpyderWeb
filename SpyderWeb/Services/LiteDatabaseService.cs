using LiteDB;
using SpyderWeb.Data.Tags;
using SpyderWeb.Helpers;
using System.Collections.Generic;

namespace SpyderWeb.Services
{
    public class LiteDatabaseService : IDatabaseService
    {
        private readonly LiteDatabase _database;
        public LiteDatabaseService()
        {
            _database = new LiteDatabase($"{SolutionHelper.GetConfigRoot()}/spyder.db");
        }

        public bool AddTag(Tag tag)
        {
            _database.GetCollection<Tag>().Insert(tag);
            return true;
        }

        public IEnumerable<Tag> GetTags()
        {
            return _database.GetCollection<Tag>().FindAll();
        }

        public bool UpdateTag(Tag tag)
        {
            var tags = _database.GetCollection<Tag>();
            return tags.Update(tag);
        }

        public bool DeleteTag(Tag tag)
        {
            return _database.GetCollection<Tag>().Delete(tag.Id);
        }
    }
}
