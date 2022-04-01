using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.Core
{
    public class Student : StorageWrapper<Student>
    {
        public Client Client
        {
            get
            {
                return client;
            }
        }
        public Person PersonData
        {
            get
            {
                return personData;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Student)
                return personData.Equals(((Student)obj).PersonData);
            return false;
        }
        public override int GetHashCode()
        {
            return personData.GetHashCode();
        }
        public override string ToString()
        {
            return personData.ToString();
        }


        public static Student CreateNew(string fullName, Client client, string phoneNumber = "")
        {
            new Checker().
                AddNull(fullName, string.Format("Can not create student with NULL name")).
                AddNull(phoneNumber, string.Format("Can not create student with NULL phone number")).
                AddNull(client, string.Format("Can not create student with NULL client")).
                Check();
                        
            var student = new Student(fullName, phoneNumber, client);

            if (storage.GetAll().Contains(student))
                throw new InvalidOperationException(
                     "Creation student with same names and phone numbers is impossible"
                );

            storage.Add(student);
            return student;
        }
        public static IReadOnlyList<Student> FilterByName(string condition)
        {
            new Checker().
                AddNull(condition, "Filtering by null pattern is impossible").
                Check();

            return
                (from entity in storage.GetAll()
                 where entity.personData.FullName.ToLower().Contains(condition.ToLower())
                 select entity).ToList();
        }

        private Student(string fullName, string phoneNumber, Client client)
        {
            this.client = client;
            this.personData = new Person(fullName, phoneNumber);
        }

        private Client client;
        private Person personData;
    }

}
