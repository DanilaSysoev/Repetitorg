using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    public class Student : PersonsCollection<Student>
    {
        public Client Client
        {
            get
            {
                return client;
            }
        }

        internal Student(string fullName, string phoneNumber)
            : base(fullName, phoneNumber)
        { }

        private Client client;
    }

}
