using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Repetitorg.Core.Base;
using Repetitorg.Core.Exceptions;
using Newtonsoft.Json;

namespace Repetitorg.Core
{
    public class Task
    {
        public static int TasksCount
        {
            get
            {
                return tasks.Count;
            }
        }

        public static void Clear()
        {
            tasks.Clear();
            tasksByDate.Clear();
        }

        public static Task AddOnDate(string taskName, DateTime date)
        {
            Task task = new Task(taskName, date.Date, false, null);
            if (!tasksByDate.ContainsKey(date))
                tasksByDate.Add(date, new List<Task>());

            if (tasksByDate[date].Contains(task))
            {
                throw new InvalidOperationException(
                    string.Format("The task with name \"{0}\" has already been defined for date \"{1}\"", taskName, date)
                );
            }

            tasks.Add(task);
            tasksByDate[date].Add(task);
            return task;
        }
        public static List<Task> GetByDate(DateTime date)
        {
            return new List<Task>(tasksByDate[date.Date]);
        }
        public static List<Task> GetAll()
        {
            return new List<Task>(tasks);
        }
        public static void Remove(Task task)
        {
            tasks.Remove(task);
            tasksByDate[task.Date].Remove(task);
        }
        public static void Complete(Task task)
        {
            if (tasks.Contains(task))
                tasksByDate[task.Date].Find(t => t.Name == task.Name).completed = true;

            task.completed = true;
        }
        public static void AttachToProject(Task task, Project project)
        {
            if (task.Project == null || task.Project == project)
                task.project = project;
            else
                throw new InvalidOperationException(
                    string.Format("Task \"{0}\" already attached to \"{1}\" project", task, project)
                );
        }
        public static void Save(IStorage<Task> tasksStorage)
        {
            tasksStorage.Save(tasks);
        }
        public static void Load(IStorage<Task> tasksStorage)
        {
            tasks = tasksStorage.Load();

            tasksByDate = new Dictionary<DateTime, List<Task>>();
            foreach (var task in tasks)
            {
                if (!tasksByDate.ContainsKey(task.Date))
                    tasksByDate.Add(task.Date, new List<Task>());
                tasksByDate[task.Date].Add(task);
            }
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
            return Date.ToString() + ": " + Name;
        }


        private static List<Task> tasks;
        private static Dictionary<DateTime, List<Task>> tasksByDate;

        private string taskName;
        private DateTime date;
        private bool completed;
        private Project project;

        [JsonConstructor]
        private Task(string name, DateTime date, bool completed, Project project)
        {
            this.taskName = name;
            this.date = date;
            this.completed = completed;
            this.project = project;
        }

        static Task()
        {
            tasks = new List<Task>();
            tasksByDate = new Dictionary<DateTime, List<Task>>();
        }

        private const string DATA_PATH = "/data/tasks.json";
    }
}
