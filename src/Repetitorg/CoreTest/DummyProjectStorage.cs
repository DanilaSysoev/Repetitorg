using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.CoreTest
{
    class DummyProjectStorage : IStorage<Project>
    {
        private List<Project> projects;

        public int UpdatesCount { get; private set; }
        public int AddCount { get; private set; }

        public DummyProjectStorage()
        {
            projects = new List<Project>();
            UpdatesCount = 0;
            AddCount = 0;
        }

        public void Add(Project project)
        {
            projects.Add(project);
            AddCount += 1;
        }

        public IReadOnlyList<Project> GetAll()
        {
            return projects;
        }

        public void Remove(Project project)
        {
            projects.Remove(project);
        }

        public void Update(Project project)
        {
            UpdatesCount += 1;
        }

        public IList<Project> Filter(Predicate<Project> predicate)
        {
            return (from project in projects
                    where predicate(project)
                    select project).ToList();
        }
    }
}
