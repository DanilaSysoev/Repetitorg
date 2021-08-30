using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    class Checker
    {
        private string message = "";

        public Checker Add(object argument, string message)
        {
            if (argument == null)
                this.message += message + "\n";
            return this;
        }
        public void Check()
        {
            if (message != "")
                throw new ArgumentException(message);
        }
    }
}
