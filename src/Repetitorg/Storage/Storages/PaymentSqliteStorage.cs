﻿using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class PaymentSqliteStorage : IStorage<Payment>, ILoadable
    {
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

        public void Load(string pathToDb)
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