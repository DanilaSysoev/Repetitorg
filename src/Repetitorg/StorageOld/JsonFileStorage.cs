using Newtonsoft.Json;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Repetitorg.Storage
{
    public class JsonFileStorage<T> : IStorage<T>
    {
        private string path;

        public JsonFileStorage(string path)
        {
            this.path = path;
        }

        public void Save(List<T> objects)
        {
            var dataPath = Path.Combine(path + DATA_PATH);
            if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(dataPath));

            var data = JsonConvert.SerializeObject(objects, Formatting.Indented);

            using (StreamWriter writer = new StreamWriter(dataPath))
                writer.Write(data);
        }

        public List<T> Load()
        {
            var data = "";
            var dataPath = Path.Combine(path + DATA_PATH);
            if (!File.Exists(dataPath))
                Save(new List<T>());

            using (StreamReader reader = new StreamReader(dataPath))
                data = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<T>>(data);
        }

        private static string DATA_PATH = "/data/" + typeof(T).Name.ToLower() + "_data.json";
    }
}
