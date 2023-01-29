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
        private string pathToDb;
        private NoteBufferSqliteStorage noteStorage;

        public PaymentSqliteStorage(NoteBufferSqliteStorage noteStorage)
        {
            payments = new Dictionary<long, Payment>();
            this.noteStorage = noteStorage;
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

        public override void Load(string pathToDb)
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
