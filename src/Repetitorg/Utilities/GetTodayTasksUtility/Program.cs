using Repetitorg.Core;
using Repetitorg.Storage;
using Repetitorg.UtilitiesSharedService;
using System;
using System.Collections.Generic;

namespace Repetitorg.GetTodayTasksUtility
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
                ConsoleServices.ExceptionHandle("gettdy", e);
            }
        }

        private static void MainProcess(string[] args)
        {
            if (args.Length > 0)
            {
                Console.WriteLine("Please, use\ngettdy\nformat");
                return;
            }

            JsonFileStorage<Task> storage =
                new JsonFileStorage<Task>(
                    ConsoleServices.Config.GetField(ConsoleServices.DATA_PATH)
                );
            Task.Load(storage);

            List<Task> tasks = Task.GetByDate(DateTime.Now.Date);

            PrintTasks(tasks);
        }

        private static void PrintTasks(List<Task> tasks)
        {
            int num = 1;
            foreach (var task in tasks)
            {
                if (task.Completed)
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("{0, 2}. {1}", num++, task.Name);
                Console.ResetColor();
            }
        }
    }
}
