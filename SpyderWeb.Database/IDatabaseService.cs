using SpyderWeb.Models;
using System.Collections.Generic;

namespace SpyderWeb.Database
{
    public interface IDatabaseService<T> where T : IDatabaseObject
    {
        bool Add(T obj);
        IEnumerable<T> GetAll();
        bool Update(T obj);
        bool Delete(T obj);
    }
}
