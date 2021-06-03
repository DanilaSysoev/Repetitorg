using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    class NullChecker
    {
        private string message = "";

        public void Add(object argument, string message)
        {
            if (argument == null)
                this.message += message + "\n";
        }
        public static void Check(object argument, string message)
        {
            if (message != "")
                throw new ArgumentException(message);
        }
    }
}
