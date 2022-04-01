using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.Core
{
    public class Project
    {
        public static int Count
        {
            get
            {
                return projects.GetAll().Count;
            }
        }
        public static Project CreateNew(string name)
        {
            new Checker().
                AddNull(name, "Can't create project with NULL name").
                Check();

            Project project = new Project(name, false);
            if (projects.GetAll().Contains(project))
                throw new InvalidOperationException(string.Format("Project with name \"{0}\" already exist", name));

            projects.Add(project);

            return project;
        }
        public static IReadOnlyList<Project> GetAll()
        {
            return projects.GetAll();
        }
        public static void Remove(Project project)
        {
            projects.Remove(project);
        }
        public static List<Project> FindByName(string subname)
        {
            new Checker().
                AddNull(subname, "Filter pattern can't be null").
                Check();

            return (from project in projects.GetAll()
                    where project.Name.ToLower().Contains(subname.ToLower())
                    select project).ToList();
        }
        public static void Complete(Project project)
        {
            project.completed = true;
            projects.Update(project);
        }
        public static void InitializeStorage(IProjectStorage storage)
        {
            projects = storage;
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


        private string name;
        private bool completed;

        private Project(string name, bool completed)
        {
            this.name = name;
            this.completed = completed;
        }

        private static IProjectStorage projects;
    }
}
