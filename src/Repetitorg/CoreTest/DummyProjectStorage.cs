using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.CoreTest
{
    class DummyProjectStorage : IProjectStorage
    {
        private List<Project> projects;

        public int UpdatesCount { get; private set; }

        public DummyProjectStorage()
        {
            projects = new List<Project>();
        }

        public void Add(Project project)
        {
            projects.Add(project);
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
    }
}
