using Repetitorg.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Repetitorg.UtilitiesSharedService
{
    public class ConsoleServices
    {
        public static Config Config { get; private set; }

        public static void ReadConfig()
        {
            Config = new Config();
            Config.Read(CONFIG_NAME);
            if (Config.GetField(DATA_PATH) == null)
            {
                Config.AddField("data_path", DEFAULT_DATA_PATH);
                Config.Write(CONFIG_NAME);
            }
        }
        public static void ExceptionHandle(string utilityName, Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            Console.WriteLine();
            Console.WriteLine("Trying create info file...");
            try
            {
                WriteInfoFile(utilityName, e);
                Console.WriteLine("OK!");
            }
            catch (Exception exc)
            {
                Console.WriteLine("Can not write info file:");
                Console.WriteLine(exc.Message);
                Console.WriteLine(exc.StackTrace);
            }
        }
        public static void PrintMenu(List<Project> projects)
        {
            Console.WriteLine("What project did you mean?");
            for (int i = 0; i < projects.Count; ++i)
            {
                if (projects[i].Completed)
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("{0, 5}. {1}", i + 1, projects[i]);
                Console.ResetColor();
            }
            Console.Write("Other. Nothing and exit\n>");
        }
        public static Project SelectionProjectMenu(List<Project> projects)
        {
            ConsoleServices.PrintMenu(projects);
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
        public static Project SelectProjectFromFilteringResult(string projectName, List<Project> projects)
        {
            Project project = null;
            if (projects.Count == 0)
            {
                Console.WriteLine(string.Format("Project \"{0}\" is not exist.", projectName));
            }
            else if (projects.Count == 1 && projects[0].Name == projectName)
                project = projects[0];
            else
                project = ConsoleServices.SelectionProjectMenu(projects);
            return project;
        }

        private static void WriteInfoFile(string utilityName, Exception e)
        {
            if (!Directory.Exists(INFO_DIR))
                Directory.CreateDirectory(INFO_DIR);
            using (StreamWriter writer = new StreamWriter(INFO_DIR +  "/" + utilityName + "_crash_" + DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss") + ".txt"))
            {
                writer.Write(e.Message);
                writer.Write(e.StackTrace);
            }
        }

        private const string INFO_DIR = "info";
        private static string CONFIG_NAME = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.json");
        public const string DATA_PATH = "data_path";
        private static string DEFAULT_DATA_PATH = 
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
