﻿using NUnit.Framework;
using Repetitorg.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repetitorg.CoreTest
{
    [TestFixture]
    class StudentTests
    {
        DummyPersonStorage<Student> students;
        DummyPersonStorage<Client> clients;
        DummyPaymentStorage payments;

        [SetUp]
        public void Initialize()
        {
            students = new DummyPersonStorage<Student>();
            clients = new DummyPersonStorage<Client>();
            payments = new DummyPaymentStorage();
            Student.InitializeStorage(students);
            Client.InitializeStorage(clients);
        }

        [TestCase]
        public void StudentsCount_EmptyStudents_StudentsCountIsZero()
        {
            Assert.AreEqual(0, Student.Count);
        }

        [TestCase]
        public void CreateNew_CreateStudentWithOnlyName_StudentsCountIncrease()
        {
            Client client = Client.CreateNew("c1");
            Student.CreateNew("Test Student", client);
            Assert.AreEqual(1, Student.Count);
        }
        [TestCase]
        public void CreateNew_CreateStudentWithOnlyName_PhoneNumberIsEmpty()
        {
            Client client = Client.CreateNew("c1");
            Student s = Student.CreateNew("Test Student", client);

            Assert.AreEqual("", s.PhoneNumber);
        }
        [TestCase]
        public void CreateNew_CreateStudentWithNameAndPhoneNumber_PhoneNumberSetCorrectly()
        {
            Client client = Client.CreateNew("c1");
            Student s = Student.CreateNew("Test Student", client, "8-999-123-45-67");

            Assert.AreEqual("8-999-123-45-67", s.PhoneNumber);
        }
        [TestCase]
        public void CreateNew_CreateStudent_ClientPropertyIsOk()
        {
            Client client = Client.CreateNew("c1");
            Student s = Student.CreateNew("Test Student", client, "8-999-123-45-67");

            Assert.AreEqual(client, s.Client);
        }
        [TestCase]
        public void CreateNew_CreateStudentWithNullName_ThrowsException()
        {
            Client client = Client.CreateNew("c1");

            var exception = Assert.Throws<ArgumentException>(
                () => Student.CreateNew(null, client)
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can not create student with null name"
            ));
        }
        [TestCase]
        public void CreateNew_CreateStudentWithNullClient_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => Student.CreateNew("s1", null)
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can not create student with null client"
            ));
        }
        [TestCase]
        public void CreateNew_CreateStudentWithNullPhoneNumber_ThrowsException()
        {
            Client client = Client.CreateNew("c1");

            var exception = Assert.Throws<ArgumentException>(
                () => Student.CreateNew("Test student", client, null)
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can not create student with null phone number"
            ));
        }
        [TestCase]
        public void CreateNew_CreateStudentsWithSameNameAndPhoneNumber_ThrowsException()
        {
            Client client = Client.CreateNew("c1");
            Student.CreateNew("Test student", client, "Test Phone");

            var exception = Assert.Throws<InvalidOperationException>(
                () => Student.CreateNew("Test student", client, "Test Phone")
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "creation student with same names and phone numbers is impossible"
            ));
        }

        [TestCase]
        public void GetAll_CreateTwo_AllReturned()
        {
            Client client = Client.CreateNew("c1");

            Student s1 = Student.CreateNew("Test student 1", client);
            Student s2 = Student.CreateNew("Test student 2", client);

            IReadOnlyList<Student> students = Student.GetAll();

            Assert.AreEqual(2, students.Count);
            Assert.IsTrue(students.Contains(s1));
            Assert.IsTrue(students.Contains(s2));
        }
        [TestCase]
        public void GetAll_CreateTwo_ReturnedCopyOfCollection()
        {
            Client client = Client.CreateNew("c1");

            Student s1 = Student.CreateNew("Test student 1", client);
            Student s2 = Student.CreateNew("Test student 2", client);

            IList<Student> students_old = new List<Student>(Student.GetAll());
            students_old.Remove(s1);
            IReadOnlyList<Student> students = Student.GetAll();

            Assert.AreEqual(2, students.Count);
            Assert.AreEqual(1, students_old.Count);
        }

        [TestCase]
        public void Equals_DifferentObjectsWithSameNameAndDifferentPhonNumbers_IsDifferent()
        {
            Client client = Client.CreateNew("c1");

            Student s1 = Student.CreateNew("Иванов Иван Иванович", client, "8-999-123-45-67");
            Student s2 = Student.CreateNew("Иванов Иван Иванович", client, "8-999-456-78-90");
            Assert.IsFalse(s1.Equals(s2));
        }
        [TestCase]
        public void Equals_EqualsWithClientWitSameNameAndPhoneNumber_IsDifferent()
        {
            Client client = Client.CreateNew("c1");

            Student s = Student.CreateNew("Иванов Иван Иванович", client, "8-999-123-45-67");
            Client c = Client.CreateNew("Иванов Иван Иванович", "8-999-123-45-67");
            Assert.IsFalse(s.Equals(c));
        }

        [TestCase]
        public void FilterByName_UseFullNameWithOneEntry_GettingOneObject()
        {
            List<Student> students = new List<Student>();
            Client client = Client.CreateNew("c1");

            students.Add(Student.CreateNew("Test student 1", client));
            students.Add(Student.CreateNew("Test student 4", client));

            students.Add(Student.CreateNew("Test student 3", client, "Phone_1"));
            students.Add(Student.CreateNew("Test student 3", client, "Phone_2"));

            var filtered_students = Student.FilterByName("Test student 1");
            Assert.AreEqual(1, filtered_students.Count);
            Assert.AreEqual(students[0], filtered_students[0]);
        }
        [TestCase]
        public void FilterByName_UsePartialNameWithOneEntry_GettingOneObject()
        {
            List<Student> students = new List<Student>();
            Client client = Client.CreateNew("c1");

            students.Add(Student.CreateNew("Test student 1", client));
            students.Add(Student.CreateNew("Test student 4", client));

            students.Add(Student.CreateNew("Test student 3", client, "Phone_1"));
            students.Add(Student.CreateNew("Test student 3", client, "Phone_2"));

            var filtered_students = Student.FilterByName("t 1");

            Assert.AreEqual(1, filtered_students.Count);
            Assert.AreEqual(students[0], filtered_students[0]);
        }
        [TestCase]
        public void FilterByName_UseLowercaseFullNameWithOneEntry_GettingOneObject()
        {
            List<Student> allStudents = new List<Student>();
            Client client = Client.CreateNew("c1");

            allStudents.Add(Student.CreateNew("Test student 1", client));
            allStudents.Add(Student.CreateNew("Test student 4", client));

            allStudents.Add(Student.CreateNew("Test student 3", client, "Phone_1"));
            allStudents.Add(Student.CreateNew("Test student 3", client, "Phone_2"));

            var students = Student.FilterByName("test student 1");
            Assert.AreEqual(1, students.Count);
            Assert.AreEqual(allStudents[0], students[0]);
        }
        [TestCase]
        public void FilterByName_UseLowercasePartialNameWithOneEntry_GettingOneObject()
        {
            List<Student> allStudents = new List<Student>();
            Client client = Client.CreateNew("c1");

            allStudents.Add(Student.CreateNew("Ivanov Ivan Ivanych", client));
            allStudents.Add(Student.CreateNew("Test student 4", client));

            allStudents.Add(Student.CreateNew("Test student 3", client, "Phone_1"));
            allStudents.Add(Student.CreateNew("Test student 3", client, "Phone_2"));

            var students = Student.FilterByName("ivanov");
            Assert.AreEqual(1, students.Count);
            Assert.AreEqual(allStudents[0], students[0]);
        }
        [TestCase]
        public void FilterByName_UseFullNameWithTwoEntry_GettingTwoObject()
        {
            List<Student> allStudents = new List<Student>();
            Client client = Client.CreateNew("c1");

            allStudents.Add(Student.CreateNew("Ivanov Ivan Ivanych", client));
            allStudents.Add(Student.CreateNew("Test student 4", client));

            allStudents.Add(Student.CreateNew("Test student 3", client, "Phone_1"));
            allStudents.Add(Student.CreateNew("Test student 3", client, "Phone_2"));

            var students = Student.FilterByName("Test student 3");
            Assert.AreEqual(2, students.Count);
            Assert.IsTrue(students.Contains(allStudents[2]));
            Assert.IsTrue(students.Contains(allStudents[3]));
        }
        [TestCase]
        public void FilterByName_UsePartialNameWithThreeEntry_GettingThreeObject()
        {
            List<Student> allStudents = new List<Student>();
            Client client = Client.CreateNew("c1");

            allStudents.Add(Student.CreateNew("Ivanov Ivan Ivanych", client));
            allStudents.Add(Student.CreateNew("Test student 4", client));

            allStudents.Add(Student.CreateNew("Test student 3", client, "Phone_1"));
            allStudents.Add(Student.CreateNew("Test student 3", client, "Phone_2"));

            var students = Student.FilterByName("Test");
            Assert.AreEqual(3, students.Count);
            Assert.IsTrue(students.Contains(allStudents[1]));
            Assert.IsTrue(students.Contains(allStudents[2]));
            Assert.IsTrue(students.Contains(allStudents[3]));
        }
        [TestCase]
        public void FilterByName_UseLowercasePartialNameWithThreeEntry_GettingThreeObject()
        {
            List<Student> allStudents = new List<Student>();
            Client client = Client.CreateNew("c1");

            allStudents.Add(Student.CreateNew("Ivanov Ivan Ivanych", client));
            allStudents.Add(Student.CreateNew("Test student 4", client));

            allStudents.Add(Student.CreateNew("Test student 3", client, "Phone_1"));
            allStudents.Add(Student.CreateNew("Test student 3", client, "Phone_2"));

            var students = Student.FilterByName("test");
            Assert.AreEqual(3, students.Count);
            Assert.IsTrue(students.Contains(allStudents[1]));
            Assert.IsTrue(students.Contains(allStudents[2]));
            Assert.IsTrue(students.Contains(allStudents[3]));
        }
        [TestCase]
        public void FilterByName_filterByNull_ThrowsException()
        {
            Client client = Client.CreateNew("c1");

            Student.CreateNew("Ivanov Ivan Ivanych", client);
            Student.CreateNew("Test student 4", client);
            Student.CreateNew("Test student 3", client, "Phone_1");
            Student.CreateNew("Test student 3", client, "Phone_2");

            var exception = Assert.Throws<ArgumentException>(
                () => Student.FilterByName(null)
            );

            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "filtering by null pattern is impossible"
                )
            );
        }

        [TestCase]
        public void ToString_SimpleStudent_ContainsFullName()
        {
            Client client = Client.CreateNew("c1");
            Student s = Student.CreateNew("Ivanov Ivan Ivanych", client);
            Assert.IsTrue(s.ToString().Contains("Ivanov Ivan Ivanych"));
        }
    }
}
