using Repetitorg.Core;
using Repetitorg.Core.Base;
using Storage.SQLite.Storages.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class StudentSqliteStorage : SqliteLoadable<Student>
    {
        public StudentSqliteStorage(SqliteDatabase database)
            : base(database, "Student")
        {
        }

        public override long Add(Student entity)
        {
            throw new NotImplementedException();
        }

        public override void Load()
        {
        }

        public override void Remove(Student entity)
        {
            throw new NotImplementedException();
        }

        public override void Update(Student entity)
        {
            throw new NotImplementedException();
        }
    }
}
