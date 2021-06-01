using Repetitorg.Core;
using Repetitorg.Storage;
using Repetitorg.UtilitiesSharedService;
using System;
using System.Collections.Generic;

namespace prs
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
                ConsoleServices.ExceptionHandle("prs", e);
            }
        }

        private static void MainProcess(string[] args)
        {
            JsonFileStorage<Project> storage =
                new JsonFileStorage<Project>(
                    ConsoleServices.Config.GetField(ConsoleServices.DATA_PATH)
                );
            Project.Load(storage);

            List<Project> projects = null;
            if(args.Length == 0)
                projects = Project.GetAll();
            else if(args.Length == 1)
                projects = Project.FindByName(args[0]);
            else
            {
                Console.WriteLine("Please, use");
                Console.WriteLine("prs [part_of_name]");
                Console.WriteLine("format");
                return;
            }
            PrintProjects(projects);
        }

        private static void PrintProjects(List<Project> projects)
        {
            foreach(var project in projects)
            {
                if (project.Completed)
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(project);
                Console.ResetColor();
            }
        }
    }
}
