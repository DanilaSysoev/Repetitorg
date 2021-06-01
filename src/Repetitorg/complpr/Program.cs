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
                ConsoleServices.ExceptionHandle("prs", e);
            }
        }

        private static void MainProcess(string[] args)
        {

        }
    }
}
