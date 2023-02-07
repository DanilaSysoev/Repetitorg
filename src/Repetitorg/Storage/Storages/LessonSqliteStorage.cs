using Repetitorg.Core;
using Repetitorg.Core.Base;
using Storage.SQLite.Storages.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class LessonSqliteStorage : SqliteLoadable, IStorage<Lesson>
    {
        private Dictionary<long, Lesson> lessons;

        public LessonSqliteStorage(SqliteDatabase database)
            : base(database)
        {
            lessons = new Dictionary<long, Lesson>();
        }

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

        public override void Load()
        {
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
