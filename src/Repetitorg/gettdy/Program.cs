using Repetitorg.UtilitiesSharedService;
using System;

namespace Repetitorg.gettdy
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
        }
    }
}
