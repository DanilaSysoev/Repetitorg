using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.Core
{
    public class Project : StorageWrapper<Project>
    {
        public static Project CreateNew(string name)
        {
            Project project = new Project(name, false);
            CheckConditionsForCreateNew(name, project);

            storage.Add(project);

            return project;
        }
        private static void CheckConditionsForCreateNew(string name, Project project)
        {
            new Checker()
                .AddNull(name, "Can't create project with NULL name")
                .Check();
            new Checker()
                .Add(project => storage.GetAll().Contains(project),
                     project,
                     string.Format("Project with name \"{0}\" already exist", name))
                .Check(message => new InvalidOperationException(message));
        }

        public static List<Project> FindByName(string subname)
        {
            CheckConditionsForFindByName(subname);

            return (from project in storage.GetAll()
                    where project.Name.ToLower().Contains(subname.ToLower())
                    select project).ToList();
        }
        private static void CheckConditionsForFindByName(string subname)
        {
            new Checker().
                AddNull(subname, "Filter pattern can't be null").
                Check();
        }

        public void Complete()
        {
            completed = true;
            storage.Update(this);
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
    }
}
