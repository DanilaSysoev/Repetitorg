using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite
{
    class TaskSqliteStorage : IStorage<Task>
    {
        public void Add(Task entity)
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
