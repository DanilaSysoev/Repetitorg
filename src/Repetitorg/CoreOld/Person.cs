using Repetitorg.Core.Base;
using Repetitorg.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    public class Person
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
            set
            {
                if (value == null)
                    throw new InvalidPhoneNumberException("PhoneNumber can't be null");
                phoneNumber = value;
            }
        }

        internal Person(string fullName, string phoneNumber)
        {
            this.fullName = fullName;
            this.phoneNumber = phoneNumber;
        }

        public override bool Equals(object obj)
        {
            if (obj is Person)
            {
                Person client = (Person)obj;
                return client.PhoneNumber == PhoneNumber && 
                       client.FullName == FullName && 
                       GetType().Equals(obj.GetType());
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (fullName.GetHashCode() + phoneNumber.GetHashCode()) * 31;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", fullName, phoneNumber);
        }
    }
}
