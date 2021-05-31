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
                return projects.Count;
            }
        }
        public static void Add(string name)
        {
            projects.Add(new Project(name));
        }
        public static void Clear()
        {
            projects.Clear();
        }


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


        private static List<Project> projects;
        static Project()
        {
            projects = new List<Project>();
        }


        private string name;

        [JsonConstructor]
        private Project(string name)
        {
            this.name = name;
        }
    }
}
