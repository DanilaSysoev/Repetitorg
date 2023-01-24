using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class ProjectSqliteStorage : IStorage<Project>, ILoadable
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

        public void Load(string pathToDb)
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
