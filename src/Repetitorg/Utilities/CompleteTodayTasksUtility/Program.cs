using Repetitorg.Core;
using Repetitorg.Storage;
using Repetitorg.UtilitiesSharedService;
using System;
using System.Collections.Generic;

namespace Repetitorg.CompleteTodayTasksUtility
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
                ConsoleServices.ExceptionHandle("compltdy", e);
            }
        }

        private static void MainProcess(string[] args)
        {
            if (args.Length > 0)
            {
                Console.WriteLine("Please, use\ncompltdy\nformat");
                return;
            }

            JsonFileStorage<Task> storage =
                new JsonFileStorage<Task>(
                    ConsoleServices.Config.GetField(ConsoleServices.DATA_PATH)
                );
            Task.Load(storage);

            List<Task> tasks = Task.GetByDate(DateTime.Now.Date);

            PrintTasks(tasks);
            CompleteSelectedTasks(tasks);
            Task.Save(storage);
        }

        private static void CompleteSelectedTasks(List<Task> tasks)
        {
            Console.Write("Enter the numbers of the tasks you want to complete:\n>");
            var tokens = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for(int i = 0; i < tokens.Length; ++i)
            {
                int index = -1;
                if(int.TryParse(tokens[i], out index) && index > 0 && index <= tasks.Count)
                {
                    Task.Complete(tasks[index - 1]);
                    Console.WriteLine("Task {0} complete!", tasks[index - 1].Name);
                }
                else
                {
                    Console.WriteLine("{0} is invalid number of task.", tokens[i]);
                }
            }
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
