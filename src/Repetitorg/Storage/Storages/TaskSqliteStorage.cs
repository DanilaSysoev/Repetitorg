using Repetitorg.Core;
using Repetitorg.Core.Base;
using Storage.SQLite.Storages.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class TaskSqliteStorage : SqliteLoadable, IStorage<Task>
    {
        public long Add(Task entity)
        {
            throw new NotImplementedException();
        }

        public IList<Task> Filter(Predicate<Task> predicate)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Task> GetAll()
        {
            throw new NotImplementedException();
        }

        public override void Load(string pathToDb)
        {
        }

        public void Remove(Task entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Task entity)
        {
            throw new NotImplementedException();
        }
    }
}
