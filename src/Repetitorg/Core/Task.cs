using System;
using System.Collections.Generic;
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

        public static void AddTomorrowTask(string taskName)
        {
            tasks.Add(new Task(taskName, DateTime.Now.Date.AddDays(1)));
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
