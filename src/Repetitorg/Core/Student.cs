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

        public static Student CreateNew(string fullName, Client client, string phoneNumber = "")
        {
            new Checker().
                AddNull(fullName, string.Format("Can not create student with NULL name")).
                AddNull(phoneNumber, string.Format("Can not create student with NULL phone number")).
                AddNull(client, string.Format("Can not create student with NULL client")).
                Check();
                        
            var student = new Student(fullName, phoneNumber, client);

            if (entities.Contains(student))
                throw new InvalidOperationException(
                     "Creation student with same names and phone numbers is impossible"
                );

            entities.Add(student);
            return student;
        }

        internal Student(string fullName, string phoneNumber, Client client)
            : base(fullName, phoneNumber)
        {
            this.client = client;
        }

        private Client client;
    }

}
