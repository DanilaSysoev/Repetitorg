using Repetitorg.Core.Base;
using Repetitorg.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    public class Person
    {
        private FullName fullName;
        private PhoneNumber phoneNumber;

        public FullName FullName
        {
            get
            {
                return fullName;
            }
        }
        public PhoneNumber PhoneNumber
        {
            get
            {
                return phoneNumber;
            }
            set
            {
                if (value == null)
                    throw new InvalidPhoneNumberException("PhoneNumber can't be null");
                phoneNumber = value;
            }
        }

        internal Person(FullName fullName, PhoneNumber phoneNumber)
        {
            this.fullName = fullName;
            this.phoneNumber = phoneNumber;
        }

        public override bool Equals(object obj)
        {
            if (obj is Person)
            {
                Person person = (Person)obj;
                return (person.PhoneNumber == null && PhoneNumber == null ||
                        (person.PhoneNumber != null && PhoneNumber != null &&
                         person.PhoneNumber.Equals(PhoneNumber))) && 
                       person.FullName.Equals(FullName) && 
                       GetType().Equals(obj.GetType());
            }
            return false;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(fullName, phoneNumber == null ? 0 : phoneNumber.GetHashCode());
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", fullName, phoneNumber == null ? "" : phoneNumber.ToString());
        }
    }
}
