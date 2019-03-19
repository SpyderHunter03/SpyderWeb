using LiteDB;
using SpyderWeb.Database.Models;
using SpyderWeb.Helpers;
using System.Collections.Generic;

namespace SpyderWeb.Database
{
    public class LiteDatabaseService<T> : IDatabaseService<T> where T : BaseDatabaseObject
    {
        private readonly LiteDatabase _database;
        public LiteDatabaseService()
        {
            _database = new LiteDatabase($"{SolutionHelper.GetConfigRoot()}/spyder.db");
        }

        public bool Add(T obj)
        {
            _database.GetCollection<T>().Insert(obj);
            return true;
        }

        public IEnumerable<T> GetAll()
        {
            return _database.GetCollection<T>().FindAll();
        }

        public bool Update(T obj)
        {
            var objs = _database.GetCollection<T>();
            return objs.Update(obj);
        }

        public bool Delete(T obj)
        {
            var objs = _database.GetCollection<T>();
            var num = objs.Delete(i => i.Id == obj.Id);
            return num != 0;
        }
    }
}
