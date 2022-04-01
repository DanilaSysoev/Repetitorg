using System;
using System.Collections.Generic;
using System.Linq;
using Repetitorg.Core.Base;

namespace Repetitorg.Core
{
    public class Task
    {
        public static int TasksCount
        {
            get
            {
                return tasks.GetAll().Count;
            }
        }

        public static Task CreateNew(string taskName, DateTime date)
        {
            new Checker().
                AddNull(taskName, "Can't add task with NULL name").
                Check();

            Task task = new Task(taskName, date.Date, false, null);
            
            if (tasks.GetByDate(date).Contains(task))
                throw new InvalidOperationException(
                    string.Format("The task with name \"{0}\" has already been defined for date \"{1}\"", taskName, date)
                );

            tasks.Add(task);
            return task;
        }
        public static IReadOnlyList<Task> GetByDate(DateTime date)
        {
            return tasks.GetByDate(date);
        }
        public static IReadOnlyList<Task> GetAll()
        {
            return tasks.GetAll();
        }
        public static void Remove(Task task)
        {
            new Checker().AddNull(task, "Task can't be null").Check();

            tasks.Remove(task);
        }
        public static void Complete(Task task)
        {
            task.completed = true;
            tasks.Update(task);
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
            tasks.Update(task);
        }

        public static IReadOnlyList<Task> GetByProject(Project project)
        {            
            return tasks.GetByProject(project);
        }
        public static IReadOnlyList<Task> GetWithoutProject()
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

        private static ITaskStorage tasks;

        public static void InitializeStorage(ITaskStorage storage)
        {
            tasks = storage;
        }
    }
}
