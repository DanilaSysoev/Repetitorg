using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.CoreTest
{
    class DummyTasksStorage : IStorage<Task>
    {
        private List<Task> tasks;
        public int UpdatesCount { get; private set; }

        public DummyTasksStorage()
        {
            tasks = new List<Task>();
        }

        public void Add(Task task)
        {
            tasks.Add(task);
        }

        public IReadOnlyList<Task> GetAll()
        {
            return tasks;
        }

        public IReadOnlyList<Task> GetByDate(DateTime date)
        {
            return (from task in tasks
                    where task.Date == date
                    select task).ToList();
        }

        public IReadOnlyList<Task> GetByProject(Project project)
        {
            return (from task in tasks
                    where task.Project == project
                    select task).ToList();
        }

        public void Remove(Task task)
        {
            tasks.Remove(task);
        }

        public void Update(Task task)
        {
            UpdatesCount += 1;
        }

        public IReadOnlyList<Task> Filter(Predicate<Task> predicate)
        {
            return (from task in tasks
                    where predicate(task)
                    select task).ToList();
        }
    }
}
