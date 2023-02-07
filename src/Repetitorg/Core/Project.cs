using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.Core
{
    public class Project : StorageWrapper<Project>, IId
    {
        public static Project CreateNew(string name)
        {
            Project project = new Project(name, false);
            CheckConditionsForCreateNew(project);

            project.Id = storage.Add(project);

            return project;
        }
        private static void CheckConditionsForCreateNew(Project project)
        {
            new Checker()
                .AddNull(project.Name, "Can't create project with NULL name")
                .Check();
            new Checker()
                .Add(project => storage.GetAll().Contains(project),
                     project,
                     string.Format("Project with name \"{0}\" already exist", project.Name))
                .Check(message => new InvalidOperationException(message));
        }

        public static Project CreateLoaded(
            long id, string name, bool completed, string note
        )
        {
            Project project = new Project(name, completed);
            project.Id = id;
            return project;
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

        public new static void Remove(Project project)
        {
            CheckConditionsForRemove(project);
            StorageWrapper<Project>.Remove(project);
        }
        public static void CheckConditionsForRemove(Project project)
        {
            new Checker()
                .Add(p => Task.GetByProject(p).Count > 0,
                     project,
                     "Can't remove project, exist attached tasks")
                .Check(message => new InvalidOperationException(message));
        }

        public void Complete()
        {
            CheckConditionsForComplete(this);            
            completed = true;
            storage.Update(this);
        }
        public static void CheckConditionsForComplete(Project project)
        {
            new Checker()
                .Add(p => Task.GetByProject(p).Any(t => !t.Completed),
                     project,
                     "Can't complete project, exist non-completed tasks")
                .Check(message => new InvalidOperationException(message));
        }

        public long Id { get; private set; }
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

            NotesUpdated += () => storage.Update(this);
        }
    }
}
