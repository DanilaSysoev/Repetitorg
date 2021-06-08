using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    class Person
    {
        private string fullName;
        private string phoneNumber;

        public string FullName
        {
            get
            {
                return fullName;
            }
        }
        public string PhoneNumber
        {
            get
            {
                return phoneNumber;
            }
        }

        internal Person(string fullName, string phoneNumber)
        {
            this.fullName = fullName;
            this.phoneNumber = phoneNumber;
        }
    }
}
