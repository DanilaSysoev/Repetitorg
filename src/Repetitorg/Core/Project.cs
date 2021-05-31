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
        public static Project Add(string name)
        {
            Project project = new Project(name);
            if (projects.Contains(project))
                throw new InvalidOperationException(string.Format("Project with name \"{0}\" already exist", name));

            projects.Add(project);

            return project;
        }
        public static List<Project> GetAll()
        {
            return new List<Project>(projects);
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


        private static HashSet<Project> projects;
        static Project()
        {
            projects = new HashSet<Project>();
        }


        private string name;

        [JsonConstructor]
        private Project(string name)
        {
            this.name = name;
        }
    }
}
