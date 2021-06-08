using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    class Person : Setupable
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

        internal override void Setup(params object[] argumenst)
        {
            fullName = (string)(argumenst[0]);
            phoneNumber = (string)(argumenst[1]);
        }
    }
}
