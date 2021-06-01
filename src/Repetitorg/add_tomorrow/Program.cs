using Repetitorg.Core;
using Repetitorg.Storage;
using System;
using System.Collections.Generic;
using System.IO;

namespace add_tomorrow
{
    class Program
    {
        private static Config config;

        static void Main(string[] args)
        {
            try
            {
                ReadConfig();
                MainProcess(args);
            }
            catch (Exception e)
            {
                ExceptionHandle(e);
            }
        }

        private static void ReadConfig()
        {
            config = new Config();
            if (!File.Exists(CONFIG_NAME))
            {
                config.AddField("data_path", DEFAULT_DATA_PATH);
                config.Write(CONFIG_NAME);
            }
            config.Read(CONFIG_NAME);
        }

        private static void MainProcess(string[] args)
        {
            if (args.Length == 3)
                AddTomorrowWithProject(args[1], args[2]);
            else if (args.Length == 2)
                AddTomorrowWithoutProject(args[1]);
            else
                ArgumentsCountError();
        }

        private static void AddTomorrowWithoutProject(string taskName)
        {
            var storage = new JsonFileStorage<Task>(config.GetField(DATA_PATH));
            Task.Load(storage);
            Task.AddOnDate(taskName, DateTime.Now.AddDays(1).Date);
            Task.Save(storage);
        }

        private static void AddTomorrowWithProject(string taskName, string projectName)
        {
            var taskStorage = new JsonFileStorage<Task>(config.GetField(DATA_PATH));
            var projectStorage = new JsonFileStorage<Project>(config.GetField(DATA_PATH));
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
                project = SelectionProjectMenu(projects);
            return project;
        }

        private static Project SelectionProjectMenu(List<Project> projects)
        {
            PrintMenu(projects);
            int projectNumber = -1;
            var input = Console.ReadLine();
            if (!int.TryParse(input, out projectNumber) ||
                projectNumber <= 0 ||
                projectNumber > projects.Count)
            {
                return null;
            }
            return projects[projectNumber - 1];
        }

        private static void PrintMenu(List<Project> projects)
        {
            Console.WriteLine("What project did you mean?");
            for (int i = 0; i < projects.Count; ++i)
            {
                if (projects[i].Completed)
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("{0, 5}. {1}", i + 1, projects[i]);
                Console.ResetColor();
            }
            Console.WriteLine("Other. Nothing\n>");
        }

        private static void ArgumentsCountError()
        {
            Console.WriteLine("Please use");
            Console.WriteLine("add_tomorrow <task_name> <project_name>");
            Console.WriteLine("or");
            Console.WriteLine("add_tomorrow <task_name>");
            Console.WriteLine("format");
        }

        private static void ExceptionHandle(Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            Console.WriteLine();
            Console.WriteLine("Trying create info file...");
            try
            {
                WriteInfoFile(e);
                Console.WriteLine("OK!");
            }
            catch(Exception exc)
            {
                Console.WriteLine("Can not write info file:");
                Console.WriteLine(exc.Message);
                Console.WriteLine(exc.StackTrace);
            }
        }

        private static void WriteInfoFile(Exception e)
        {
            if (!Directory.Exists(INFO_DIR))
                Directory.CreateDirectory(INFO_DIR);
            using (StreamWriter writer = new StreamWriter(INFO_DIR + "/add_tomorrow_crash_" + DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") + ".txt"))
            {
                writer.Write(e.Message);
                writer.Write(e.StackTrace);
            }
        }        

        private const string INFO_DIR = "info";
        private const string CONFIG_NAME = "config.json";
        private const string DATA_PATH = "data_path";
        private static string DEFAULT_DATA_PATH = Environment.CurrentDirectory;
    }
}
