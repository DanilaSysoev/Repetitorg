using Repetitorg.Core;
using Repetitorg.Storage;
using Repetitorg.UtilitiesSharedService;
using System;

namespace Repetitorg.addprs
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
                ConsoleServices.ExceptionHandle("addprs", e);
            }
        }

        private static void MainProcess(string[] args)
        {
            JsonFileStorage<Project> storage =                
                new JsonFileStorage<Project>(ConsoleServices.Config.GetField(ConsoleServices.DATA_PATH));
            Project.Load(storage);
            foreach (var project in args)
            {
                Project.Add(project);
                Project.Save(storage);
                Console.WriteLine("{0} added!", project);
            }
        }
    }
}
