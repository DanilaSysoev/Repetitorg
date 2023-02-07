using Repetitorg.Core;
using Repetitorg.Core.Base;
using Storage.SQLite.Storages.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class OrderSqliteStorage : SqliteLoadable<Order>
    {
        public OrderSqliteStorage(SqliteDatabase database)
            : base(database)
        {
        }

        public override long Add(Order entity)
        {
            throw new NotImplementedException();
        }

        public override void Load()
        {
        }

        public override void Remove(Order entity)
        {
            throw new NotImplementedException();
        }

        public override void Update(Order entity)
        {
            throw new NotImplementedException();
        }
    }
}
