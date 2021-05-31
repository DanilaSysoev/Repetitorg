using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core.Exceptions
{
    public class TaskAlreadyExistException : Exception
    {
        public TaskAlreadyExistException(string message)
            : base(message)
        { }
    }
}
