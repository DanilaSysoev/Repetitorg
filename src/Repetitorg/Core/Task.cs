using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

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

        public static void Save()
        {
            var data = JsonConvert.SerializeObject(tasks);
            using (StreamWriter writer = new StreamWriter(DATA_PATH))
                writer.Write(data);
        }
        public static void Load()
        {
            var data = "";
            using (StreamReader reader = new StreamReader(DATA_PATH))
                data = reader.ReadToEnd();
            tasks = JsonConvert.DeserializeObject<List<Task>>(data);
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

        [JsonConstructor]
        private Task(string taskName, DateTime date)
        {
            this.taskName = taskName;
            this.date = date;
        }

        static Task()
        {
            tasks = new List<Task>();
            if (!Directory.Exists(DATA_DIR))
                Directory.CreateDirectory(DATA_DIR);
        }

        private const string DATA_PATH = "/data/tasks.json";
        private const string DATA_DIR = "/data";
    }
}
