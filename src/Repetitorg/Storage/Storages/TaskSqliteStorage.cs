﻿using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class TaskSqliteStorage : IStorage<Task>, ILoadable
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

        public void Load(string pathToDb)
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