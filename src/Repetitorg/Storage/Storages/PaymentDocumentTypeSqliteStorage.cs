using Repetitorg.Core;
using Repetitorg.Core.Base;
using Storage.SQLite.Storages.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class PaymentDocumentTypeSqliteStorage :
        SqliteLoadable, IStorage<PaymentDocumentType>
    {
        public long Add(PaymentDocumentType entity)
        {
            throw new NotImplementedException();
        }

        public IList<PaymentDocumentType> Filter(Predicate<PaymentDocumentType> predicate)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<PaymentDocumentType> GetAll()
        {
            throw new NotImplementedException();
        }

        public override void Load(string pathToDb)
        {
        }

        public void Remove(PaymentDocumentType entity)
        {
            throw new NotImplementedException();
        }

        public void Update(PaymentDocumentType entity)
        {
            throw new NotImplementedException();
        }
    }
}
