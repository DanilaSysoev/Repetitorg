﻿using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class LessonSqliteStorage : IStorage<Lesson>, ILoadable
    {
        public long Add(Lesson entity)
        {
            throw new NotImplementedException();
        }

        public IList<Lesson> Filter(Predicate<Lesson> predicate)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Lesson> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Load(string pathToDb)
        {
            throw new NotImplementedException();
        }

        public void Remove(Lesson entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Lesson entity)
        {
            throw new NotImplementedException();
        }
    }
}