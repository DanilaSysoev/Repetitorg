using Repetitorg.Core;
using Repetitorg.Core.Base;
using Storage.SQLite.Storages.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class PaymentSqliteStorage : SqliteLoadable, IStorage<Payment>
    {
        private Dictionary<long, Payment> payments;

        public PaymentSqliteStorage(SqliteDatabase database)
            : base(database)
        {
            payments = new Dictionary<long, Payment>();
        }

        public long Add(Payment entity)
        {
            throw new NotImplementedException();
        }

        public IList<Payment> Filter(Predicate<Payment> predicate)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Payment> GetAll()
        {
            throw new NotImplementedException();
        }

        public override void Load()
        {
        }

        public void Remove(Payment entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Payment entity)
        {
            throw new NotImplementedException();
        }
    }
}
