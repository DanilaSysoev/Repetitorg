using System;
using System.Collections.Generic;
using System.Linq;
using Repetitorg.Core.Base;

namespace Repetitorg.Core
{
    public class Task : StorageWrapper<Task>, IId
    {
        public static Task CreateNew(string taskName, DateTime date)
        {
            Task task = new Task(taskName, date.Date, false, null);
            CheckConditionsForCreateNew(taskName, date, task);

            task.Id = storage.Add(task);
            return task;
        }
        private static void CheckConditionsForCreateNew(string taskName, DateTime date, Task task)
        {
            new Checker()
                .AddNull(taskName, "Can't add task with NULL name")
                .Check();
            new Checker()
                .Add(task => storage.Filter(t => t.Date.Equals(date.Date)).Contains(task),
                     task,
                     string.Format("The task with name \"{0}\" has already been defined for date \"{1}\"", taskName, date))
                .Check(message => new InvalidOperationException(message));
        }

        public static IList<Task> GetByDate(DateTime date)
        {
            return storage.Filter(t => t.Date.Equals(date));
        }
        public void Complete()
        {
            completed = true;
            storage.Update(this);
        }
        public void AttachToProject(Project project)
        {
            CheckConditionsForAttachToProject(project);

            this.project = project;
            storage.Update(this);
        }
        private void CheckConditionsForAttachToProject(Project project)
        {
            new Checker()
                .Add(_ => Project != project && project != null && project.Completed,
                     this,
                     "Can't attach new task to complete project")
                .Add(_ => Project != null && project != null && !Project.Equals(project),
                     this,
                     string.Format("Task \"{0}\" already attached to \"{1}\" project", this, Project))
                .Check(message => new InvalidOperationException(message));
        }

        public static IList<Task> GetByProject(Project project)
        {
            return storage.Filter(t => t.Project == project);
        }
        public static IList<Task> GetWithoutProject()
        {
            return GetByProject(null);
        }

        public string Name
        {
            get
            {
                return taskName; 
            }
        }
        public DateTime Date
        {
            get
            {
                return date;
            }
        }
        public bool Completed
        {
            get
            {
                return completed;
            }
        }
        public Project Project
        {
            get
            {
                return project;
            }
        }
        public long Id { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj is Task)
                return ((Task)obj).Name.Equals(Name) && ((Task)obj).Date.Equals(Date);
            return false;
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode() + Date.GetHashCode();
        }
        public override string ToString()
        {
            if (Project != null)
                return string.Format("({0}) {1}: {2}", Project, Date, Name);

            return Date.ToString() + ": " + Name;
        }


        private string taskName;
        private DateTime date;
        private bool completed;
        private Project project;

        private Task(string name, DateTime date, bool completed, Project project)
        {
            this.taskName = name;
            this.date = date;
            this.completed = completed;
            this.project = project;
        }
    }
}
