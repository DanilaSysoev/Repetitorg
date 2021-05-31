using Newtonsoft.Json;
using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Repetitorg.Storage
{
    public class JsonFileProjectsStorage : IProjectsStorage
    {
        private string path;

        public JsonFileProjectsStorage(string path)
        {
            this.path = path;
        }

        public void Save(List<Project> projects)
        {
            var dataPath = Path.Combine(path + DATA_PATH);
            if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(dataPath));

            var data = JsonConvert.SerializeObject(projects, Formatting.Indented);

            using (StreamWriter writer = new StreamWriter(dataPath))
                writer.Write(data);
        }

        public List<Project> Load()
        {
            var data = "";
            var dataPath = Path.Combine(path + DATA_PATH);
            using (StreamReader reader = new StreamReader(dataPath))
                data = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Project>>(data);
        }

        private const string DATA_PATH = "/data/projects.json";
    }
}
