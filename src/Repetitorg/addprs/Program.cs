using Repetitorg.UtilitesSharedService;
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
                ConsoleServices.ExceptionHandle("addprjs", e);
            }
        }

        private static void MainProcess(string[] args)
        {
        }

        private const string DATA_PATH = "data_path";
    }
}
