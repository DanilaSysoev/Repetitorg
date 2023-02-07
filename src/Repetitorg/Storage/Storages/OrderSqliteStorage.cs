using Repetitorg.Core;
using Repetitorg.Core.Base;
using Storage.SQLite.Storages.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class OrderSqliteStorage : SqliteLoadable, IStorage<Order>
    {
        private Dictionary<long, Order> orders;

        public OrderSqliteStorage(SqliteDatabase database)
            : base(database)
        {
            orders = new Dictionary<long, Order>();
        }

        public long Add(Order entity)
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

        public override void Load()
        {
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
