﻿using Repetitorg.Core.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Repetitorg.Core;

namespace Repetitorg.Storage
{
    public class JsonFileTasksStorage : ITasksStorage
    {
        private string path;

        public JsonFileTasksStorage(string path)
        {
            this.path = path;
        }

        public void Save()
        {
            var dataPath = Path.Combine(path + DATA_PATH);
            if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(dataPath));

            var data = JsonConvert.SerializeObject(Task.GetAll(), Formatting.Indented);

            using (StreamWriter writer = new StreamWriter(dataPath))
                writer.Write(data);
        }
        public void Load()
        {
            var data = "";
            var dataPath = Path.Combine(path + DATA_PATH);
            using (StreamReader reader = new StreamReader(dataPath))
                data = reader.ReadToEnd();
            var tasks = JsonConvert.DeserializeObject<List<Task>>(data);

            Task.Setup(tasks);
        }

        private const string DATA_PATH = "/data/tasks.json";
    }
}
