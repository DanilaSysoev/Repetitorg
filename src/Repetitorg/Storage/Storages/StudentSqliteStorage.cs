﻿using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite
{
    class StudentSqliteStorage : IStorage<Student>
    {
        public void Add(Student entity)
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
