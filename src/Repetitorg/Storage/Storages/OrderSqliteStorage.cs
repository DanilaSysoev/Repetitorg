using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class OrderSqliteStorage : IStorage<Order>, ILoadable
    {
        public void Add(Order entity)
        {
            throw new NotImplementedException();
        }

        public IList<Order> Filter(Predicate<Order> predicate)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Order> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Load(string pathToDb)
        {
            throw new NotImplementedException();
        }

        public void Remove(Order entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Order entity)
        {
            throw new NotImplementedException();
        }
    }
}
