using Repetitorg.Core;
using Repetitorg.Core.Base;
using Storage.SQLite.Storages.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class ProjectSqliteStorage : SqliteLoadable, IStorage<Project>
    {
        public long Add(Project entity)
        {
            throw new NotImplementedException();
        }

        public IList<Project> Filter(Predicate<Project> predicate)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Project> GetAll()
        {
            throw new NotImplementedException();
        }

        public override void Load(string pathToDb)
        {
        }

        public void Remove(Project entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Project entity)
        {
            throw new NotImplementedException();
        }
    }
}
