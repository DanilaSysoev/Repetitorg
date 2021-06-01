using Repetitorg.Core;
using Repetitorg.Storage;
using Repetitorg.UtilitiesSharedService;
using System;

namespace complpr
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
                ConsoleServices.ExceptionHandle("complpr", e);
            }
        }

        private static void MainProcess(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Please, use");
                Console.WriteLine("complpr <part_of_name>");
                Console.WriteLine("format");
                return;
            }

            CompleteSelectedProject(args);
        }

        private static void CompleteSelectedProject(string[] args)
        {
            JsonFileStorage<Project> storage =
                            new JsonFileStorage<Project>(
                                ConsoleServices.Config.GetField(ConsoleServices.DATA_PATH)
                            );
            Project.Load(storage);
            var project =
                ConsoleServices.SelectProjectFromFilteringResult(
                    args[0], Project.FindByName(args[0])
                );
            if (project != null)
                Project.Complete(project);
            Project.Save(storage);
        }
    }
}
