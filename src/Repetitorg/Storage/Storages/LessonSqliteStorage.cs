using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite
{
    class LessonSqliteStorage : IStorage<Lesson>
    {
        public void Add(Lesson entity)
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
