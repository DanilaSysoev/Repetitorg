using System;
using System.IO;

namespace add_tomorrow
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MainProcess(args);
            }
            catch (Exception e)
            {
                ExceptionHandle(e);
            }
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
            using (StreamWriter writer = new StreamWriter(INFO_DIR + "/add_tomorrow_crash_" + DateTime.Now + ".txt"))
            {
                writer.Write(e.Message);
                writer.Write(e.StackTrace);
            }
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
            throw new NotImplementedException();
        }

        private static void AddTomorrowWithProject(string taskName, string projectName)
        {
            throw new NotImplementedException();
        }

        private static void ArgumentsCountError()
        {
            Console.WriteLine("Please use");
            Console.WriteLine("add_tomorrow <task_name> <project_name>");
            Console.WriteLine("or");
            Console.WriteLine("add_tomorrow <task_name>");
            Console.WriteLine("format");
        }

        private const string INFO_DIR = "info";
    }
}
