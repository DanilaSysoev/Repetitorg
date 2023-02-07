using Repetitorg.Core;
using Repetitorg.Core.Base;
using Storage.SQLite.Storages.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class PaymentDocumentTypeSqliteStorage :
        SqliteLoadable<PaymentDocumentType>
    {
        public PaymentDocumentTypeSqliteStorage(SqliteDatabase database)
            : base(database)
        {
        }

        public override long Add(PaymentDocumentType entity)
        {
            throw new NotImplementedException();
        }

        public override void Load()
        {
        }

        public override void Remove(PaymentDocumentType entity)
        {
            throw new NotImplementedException();
        }

        public override void Update(PaymentDocumentType entity)
        {
            throw new NotImplementedException();
        }
    }
}
