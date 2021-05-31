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
            if (!tasksByDate.ContainsKey(date))
                tasksByDate.Add(date, new List<Task>());
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

        public static void Save(string path)
        {
            var dataPath = Path.Combine(path + DATA_PATH);
            if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(dataPath));

            var data = JsonConvert.SerializeObject(tasks, Formatting.Indented);

            using (StreamWriter writer = new StreamWriter(dataPath))
                writer.Write(data);
        }
        public static void Load(string path)
        {
            var data = "";
            var dataPath = Path.Combine(path + DATA_PATH);
            using (StreamReader reader = new StreamReader(dataPath))
                data = reader.ReadToEnd();
            tasks = JsonConvert.DeserializeObject<List<Task>>(data);

            tasksByDate = new Dictionary<DateTime, List<Task>>();
            foreach(var task in tasks)
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

        [JsonConstructor]
        private Task(string name, DateTime date)
        {
            this.taskName = name;
            this.date = date;
        }

        static Task()
        {
            tasks = new List<Task>();
            tasksByDate = new Dictionary<DateTime, List<Task>>();
        }

        private const string DATA_PATH = "/data/tasks.json";
    }
}
