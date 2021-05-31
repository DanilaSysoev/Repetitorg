using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Repetitorg.Core.Base;
using Core.Exceptions;
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
            Task task = new Task(taskName, date.Date, false);
            if (!tasksByDate.ContainsKey(date))
                tasksByDate.Add(date, new List<Task>());

            if (tasksByDate[date].Contains(task))
            {
                throw new TaskAlreadyExistException(
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
            task.completed = true;
        }
        public static void Setup(IEnumerable<Task> tasks)
        {
            if (Task.tasks.Count > 0)
                throw new InvalidOperationException("Setup can be calld only for clear Task collection");

            Task.tasks = new List<Task>(tasks);
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

        [JsonConstructor]
        private Task(string name, DateTime date, bool completed)
        {
            this.taskName = name;
            this.date = date;
            this.completed = completed;
        }

        static Task()
        {
            tasks = new List<Task>();
            tasksByDate = new Dictionary<DateTime, List<Task>>();
        }

        private const string DATA_PATH = "/data/tasks.json";
    }
}
