﻿using Repetitorg.Core;
using Repetitorg.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using Repetitorg.UtilitiesSharedService;

namespace Repetitorg.addtmrw
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
            Project project = SelectProjectFromFilteringResult(projectName, projects);

            if (project == null)
                return;

            var task = Task.AddOnDate(taskName, DateTime.Now.AddDays(1).Date);
            Task.AttachToProject(task, project);

            Task.Save(taskStorage);
        }

        private static Project SelectProjectFromFilteringResult(string projectName, List<Project> projects)
        {
            Project project = null;
            if (projects.Count == 0)
            {
                Console.WriteLine(string.Format("Project \"{0}\" is not exist.", projectName));
            }
            else if (projects.Count == 1)
                project = projects[0];
            else
                project = ConsoleServices.SelectionProjectMenu(projects);
            return project;
        }

        private static void ArgumentsCountError()
        {
            Console.WriteLine("Please use");
            Console.WriteLine("add_tomorrow <task_name> <project_name>");
            Console.WriteLine("or");
            Console.WriteLine("add_tomorrow <task_name>");
            Console.WriteLine("format");
        }
    }
}
