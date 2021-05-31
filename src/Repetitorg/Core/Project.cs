using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    public class Project
    {
        public static int ProjectsCount
        {
            get
            {
                return 0;
            }
        }
        public static void Add(string name)
        { }
        public static void Clear()
        { }


        public string Name
        {
            get
            {
                return name;
            }
        }

        public override bool Equals(object obj)
        {
            if(obj is Project)
                return ((Project)obj).Name == Name;
            return false;
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
        public override string ToString()
        {
            return Name;
        }

        private string name;

        [JsonConstructor]
        private Project(string name)
        {
            this.name = name;
        }
    }
}
