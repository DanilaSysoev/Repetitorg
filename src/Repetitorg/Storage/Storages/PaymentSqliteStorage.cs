using Repetitorg.Core;
using Repetitorg.Core.Base;
using Storage.SQLite.Storages.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class PaymentSqliteStorage : SqliteLoadable<Payment>
    {
        public PaymentSqliteStorage(SqliteDatabase database)
            : base(database)
        {
        }

        public override long Add(Payment entity)
        {
            throw new NotImplementedException();
        }

        public override void Load()
        {
        }

        public override void Remove(Payment entity)
        {
            throw new NotImplementedException();
        }

        public override void Update(Payment entity)
        {
            throw new NotImplementedException();
        }
    }
}
