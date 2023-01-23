using Microsoft.Data.Sqlite;
using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    public class ClientSqliteStorage : IStorage<Client>, ILoadable
    {
        public void Add(Client entity)
        {
            throw new NotImplementedException();
        }

        public IList<Client> Filter(Predicate<Client> predicate)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Client> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Load(string pathToDb)
        {
            throw new NotImplementedException();
        }

        public void Remove(Client entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Client entity)
        {
            throw new NotImplementedException();
        }
    }
}
