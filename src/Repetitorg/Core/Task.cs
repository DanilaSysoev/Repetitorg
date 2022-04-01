using System;
using System.Collections.Generic;
using System.Linq;
using Repetitorg.Core.Base;

namespace Repetitorg.Core
{
    public class Task : StorageWrapper<Task>
    {
        public static Task CreateNew(string taskName, DateTime date)
        {
            new Checker().
                AddNull(taskName, "Can't add task with NULL name").
                Check();

            Task task = new Task(taskName, date.Date, false, null);
            
            if (storage.Filter(t => t.Date.Equals(date)).Contains(task))
                throw new InvalidOperationException(
                    string.Format("The task with name \"{0}\" has already been defined for date \"{1}\"", taskName, date)
                );

            storage.Add(task);
            return task;
        }
        public static IList<Task> GetByDate(DateTime date)
        {
            return storage.Filter(t => t.Date.Equals(date));
        }
        public static void Complete(Task task)
        {
            task.completed = true;
            storage.Update(task);
        }
        public static void AttachToProject(Task task, Project project)
        {
            new Checker().AddNull(task, "Task can't be null").Check();

            if (task.Project != project && project != null && project.Completed)
                throw new InvalidOperationException(
                    "Can't attach new task to complete project"
                );

            if (task.Project == null || project == null)
                task.project = project;
            else if(!task.Project.Equals(project))
                throw new InvalidOperationException(
                    string.Format("Task \"{0}\" already attached to \"{1}\" project", task, task.Project)
                );
            storage.Update(task);
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
