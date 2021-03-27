using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core.Exceptions
{
    public class InvalidPhoneNumberException : Exception
    {
        public InvalidPhoneNumberException(string message)
            : base(message)
        { }
    }
}
