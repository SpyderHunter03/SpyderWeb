using SpyderWeb.Database.Models;
using System;
using System.Collections.Generic;

namespace SpyderWeb.Database
{
    public interface IDatabaseService<T> where T : BaseDatabaseObject
    {
        bool Add(T obj);
        IEnumerable<T> GetAll();
        bool Update(T obj);
        bool Delete(T obj);
    }
}
