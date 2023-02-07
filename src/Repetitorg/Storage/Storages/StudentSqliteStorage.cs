using Repetitorg.Core;
using Repetitorg.Core.Base;
using Storage.SQLite.Storages.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class StudentSqliteStorage : SqliteLoadable, IStorage<Student>
    {
        private Dictionary<long, Student> students;

        public StudentSqliteStorage(SqliteDatabase database)
            : base(database)
        {
            students = new Dictionary<long, Student>();
        }

        public long Add(Student entity)
        {
            throw new NotImplementedException();
        }

        public IList<Student> Filter(Predicate<Student> predicate)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Student> GetAll()
        {
            throw new NotImplementedException();
        }

        public override void Load()
        {
        }

        public void Remove(Student entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Student entity)
        {
            throw new NotImplementedException();
        }
    }
}
