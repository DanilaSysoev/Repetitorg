using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite
{
    class PaymentSqliteStorage : IStorage<Payment>
    {
        public void Add(Payment entity)
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
