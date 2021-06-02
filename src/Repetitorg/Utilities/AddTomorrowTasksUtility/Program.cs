using Repetitorg.Core;
using Repetitorg.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using Repetitorg.UtilitiesSharedService;

namespace Repetitorg.AddTomorrowTasksUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ConsoleServices.ReadConfig();
                MainProcess(args);
            }
            catch (Exception e)
            {
                ConsoleServices.ExceptionHandle("addtmrw", e);
            }
        }

        private static void MainProcess(string[] args)
        {
            if (args.Length == 2)
                AddTomorrowWithProject(args[0], args[1]);
            else if (args.Length == 1)
                AddTomorrowWithoutProject(args[0]);
            else
                ArgumentsCountError();
        }

        private static void AddTomorrowWithoutProject(string taskName)
        {
            var storage = new JsonFileStorage<Task>(ConsoleServices.Config.GetField(ConsoleServices.DATA_PATH));
            Task.Load(storage);
            Task.AddOnDate(taskName, DateTime.Now.AddDays(1).Date);
            Task.Save(storage);
        }

        private static void AddTomorrowWithProject(string taskName, string projectName)
        {
            var taskStorage = new JsonFileStorage<Task>(
                ConsoleServices.Config.GetField(ConsoleServices.DATA_PATH)
            );
            var projectStorage = new JsonFileStorage<Project>(
                ConsoleServices.Config.GetField(ConsoleServices.DATA_PATH)
            );
            Task.Load(taskStorage);
            Project.Load(projectStorage);

            var projects = Project.FindByName(projectName);
            Project project = 
                ConsoleServices.SelectProjectFromFilteringResult(projectName, projects);

            if (project == null)
                return;

            var task = Task.AddOnDate(taskName, DateTime.Now.AddDays(1).Date);
            Task.AttachToProject(task, project);

            Task.Save(taskStorage);
        }

        private static void ArgumentsCountError()
        {
            Console.WriteLine("Please use");
            Console.WriteLine("addtmrw <task_name> <part_of_project_name>");
            Console.WriteLine("or");
            Console.WriteLine("addtmrw <task_name>");
            Console.WriteLine("format");
        }
    }
}
