using System;
using System.Collections.Generic;
using System.IO;
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
        private const string CONFIG_NAME = "config.json";
        private const string DATA_PATH = "data_path";
        private static string DEFAULT_DATA_PATH = Environment.CurrentDirectory;
    }
}
