using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
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
        }

        public static Task AddTomorrow(string taskName)
        {
            return AddOnDate(taskName, DateTime.Today.AddDays(1));
        }
        public static Task AddOnDate(string taskName, DateTime date)
        {
            Task task = new Task(taskName, date);
            tasks.Add(task);
            return task;
        }
        public static List<Task> GetByDate(DateTime dateTime)
        {
            return (from task in tasks
                    where task.Date == dateTime
                    select task).ToList();
        }
        public static List<Task> GetTomorrowTasks()
        {
            return (from task in tasks
                    where task.Date == DateTime.Today.AddDays(1)
                    select task).ToList();
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
        

        private static List<Task> tasks;
        private string taskName;
        private DateTime date;

        private Task(string taskName, DateTime date)
        {
            this.taskName = taskName;
            this.date = date;
        }

        static Task()
        {
            tasks = new List<Task>();
        }
    }
}
