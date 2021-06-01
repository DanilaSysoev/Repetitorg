using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Repetitorg.Config
{
    public class Configurations
    {
        private Dictionary<string, string> values;

        public Configurations()
        {
            values = new Dictionary<string, string>();
        }

        public void AddField(string name, string value)
        {
            values.Add(name, value);
        }
        public string GetField(string name)
        {
            if (values.ContainsKey(name))
                return values[name];
            return null;
        }

        public void Read(string filename)
        {
            values = new Dictionary<string, string>();
            if (!File.Exists(filename))
                Write(filename);

            string configData = "";
            using (StreamReader reader = new StreamReader(filename))
                configData = reader.ReadToEnd();

            values = JsonConvert.DeserializeObject<Dictionary<string, string>>(configData);
        }
        public void Write(string filename)
        {
            string configData = JsonConvert.SerializeObject(values, Formatting.Indented);
            using (StreamWriter writer = new StreamWriter(filename))
                writer.Write(configData);
        }
    }
}
