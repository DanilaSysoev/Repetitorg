using Repetitorg.Core;
using Repetitorg.Core.Base;
using Storage.SQLite.Storages.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class LessonSqliteStorage : SqliteLoadable<Lesson>
    {
        public LessonSqliteStorage(SqliteDatabase database)
            : base(database)
        {
        }

        public override long Add(Lesson entity)
        {
            throw new NotImplementedException();
        }

        public override void Load()
        {
        }

        public override void Remove(Lesson entity)
        {
            throw new NotImplementedException();
        }

        public override void Update(Lesson entity)
        {
            throw new NotImplementedException();
        }
    }
}
