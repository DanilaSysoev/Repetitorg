using Newtonsoft.Json;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Project project = new Project(name, false);
            if (projects.Contains(project))
                throw new InvalidOperationException(string.Format("Project with name \"{0}\" already exist", name));

            projects.Add(project);

            return project;
        }
        public static List<Project> GetAll()
        {
            return new List<Project>(projects);
        }
        public static void Remove(Project project)
        {
            projects.Remove(project);
        }
        public static List<Project> FindByName(string subname)
        {
            return (from project in projects
                    where project.Name.ToLower().Contains(subname.ToLower())
                    select project).ToList();
        }
        public static void Complete(Project project)
        {
            if(projects.Contains(project))
                projects.First(p => p.Name == project.Name).completed = true;

            project.completed = true;
        }
        public static void Save(IStorage<Project> projectsStorage)
        {
            projectsStorage.Save(projects.ToList());        
        }
        public static void Load(IStorage<Project> projectsStorage)
        {
            projects = new HashSet<Project>(projectsStorage.Load());
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
        public bool Completed
        {
            get
            {
                return completed;
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
        private bool completed;

        [JsonConstructor]
        private Project(string name, bool completed)
        {
            this.name = name;
            this.completed = completed;
        }
    }
}
