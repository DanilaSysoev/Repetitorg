using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.Core
{
    public class Student : StorageWrapper<Student>, IId
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
        public long Id { get; private set; }

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


        public static Student CreateNew(
            FullName fullName, Client client, PhoneNumber phoneNumber = null
        )
        {
            var student = new Student(fullName, phoneNumber, client);
            CheckConditionsForCreateNew(student);

            student.Id = storage.Add(student);
            return student;
        }
        private static void CheckConditionsForCreateNew(Student student)
        {
            new Checker()
                .AddNull(
                    student.PersonData.FullName,
                    "Can not create student with NULL name")
                .AddNull(
                    student.Client,
                    "Can not create student with NULL client")
                .Check();
            new Checker()
                .Add(student => storage.GetAll().Contains(student),
                     student,
                     "Creation student with same names and phone numbers is impossible")
                .Check(message => new InvalidOperationException(message));
        }

        public static Student CreateLoaded(
            long id, FullName fullName, Client client, PhoneNumber phoneNumber
        )
        {
            var student = new Student(fullName, phoneNumber, client);
            student.Id = id;
            return student;
        }

        public static IReadOnlyList<Student> FilterByName(string condition)
        {
            CheckConditionsForFilterByName(condition);

            return
                (from entity in storage.GetAll()
                 where entity.personData.FullName.ToString().ToLower().Contains(condition.ToLower())
                 select entity).ToList();
        }
        private static void CheckConditionsForFilterByName(string condition)
        {
            new Checker()
                .AddNull(condition, "Filtering by null pattern is impossible")
                .Check();
        }

        private Student(FullName fullName, PhoneNumber phoneNumber, Client client)
        {
            this.client = client;
            this.personData = new Person(fullName, phoneNumber);
            personData.Update += () => storage.Update(this);
        }

        private Client client;
        private Person personData;
    }

}
