using NUnit.Framework;
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
        [TearDown]
        public static void Clear()
        {
            Student.Clear();
            Client.Clear();            
        }

        [TestCase]
        public static void StudentsCount_EmptyStudents_StudentsCountIsZero()
        {
            Assert.AreEqual(0, Student.Count);
        }

        [TestCase]
        public static void CreateNew_CreateStudentWithOnlyName_StudentsCountIncrease()
        {
            Student.CreateNew("Test Student");
            Assert.AreEqual(1, Student.Count);
        }
        [TestCase]
        public static void CreateNew_CreateStudentWithOnlyName_PhoneNumberIsEmpty()
        {
            Student s = Student.CreateNew("Test Student");

            Assert.AreEqual("", s.PhoneNumber);
        }
        [TestCase]
        public static void CreateNew_CreateStudentWithNameandPhoneNumber_PhoneNumberSetCorrectly()
        {
            Student s = Student.CreateNew("Test Student", "8-999-123-45-67");

            Assert.AreEqual("8-999-123-45-67", s.PhoneNumber);
        }
        [TestCase]
        public static void CreateNew_CreateStudentWithNullName_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => Student.CreateNew(null)
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can not create student with null name"
            ));
        }
        [TestCase]
        public static void CreateNew_CreateStudentWithNullPhoneNumber_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => Student.CreateNew("Test student", null)
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can not create student with null phone numbe"
            ));
        }
        [TestCase]
        public static void CreateNew_CreateStudentsWithSameNameAndPhoneNumber_ThrowsException()
        {
            Student.CreateNew("Test student", "Test Phone");

            var exception = Assert.Throws<InvalidOperationException>(
                () => Student.CreateNew("Test student", "Test Phone")
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "creation student with same names and phone numbers is impossible"
            ));
        }

        [TestCase]
        public void GetAll_CreateTwo_AllReturned()
        {
            Student s1 = Student.CreateNew("Test student 1");
            Student s2 = Student.CreateNew("Test student 2");

            IList<Student> students = Student.GetAll();

            Assert.AreEqual(2, students.Count);
            Assert.IsTrue(students.Contains(s1));
            Assert.IsTrue(students.Contains(s2));
        }
        [TestCase]
        public void GetAll_CreateTwo_ReturnedCopyOfCollection()
        {
            Student s1 = Student.CreateNew("Test student 1");
            Student s2 = Student.CreateNew("Test student 2");

            IList<Student> students_old = Student.GetAll();
            students_old.Remove(s1);
            IList<Student> students = Student.GetAll();

            Assert.AreEqual(2, students.Count);
            Assert.AreEqual(1, students_old.Count);
        }

        [TestCase]
        public void Equals_DifferentObjectsWithSameNameAndDifferentPhonNumbers_IsDifferent()
        {
            Student s1 = Student.CreateNew("Иванов Иван Иванович", "8-999-123-45-67");
            Student s2 = Student.CreateNew("Иванов Иван Иванович", "8-999-456-78-90");
            Assert.IsFalse(s1.Equals(s2));
        }
        [TestCase]
        public void Equals_EqualsWithClientWitSameNameAndPhoneNumber_IsDifferent()
        {
            Student s = Student.CreateNew("Иванов Иван Иванович", "8-999-123-45-67");
            Client c = Client.CreateNew("Иванов Иван Иванович", "8-999-123-45-67");
            Assert.IsFalse(s.Equals(c));
        }

        [TestCase]
        public void FilterByName_UseFullNameWithOneEntry_GettingOneObject()
        {
            List<Student> students = new List<Student>();

            students.Add(Student.CreateNew("Test student 1"));
            students.Add(Student.CreateNew("Test student 4"));

            students.Add(Student.CreateNew("Test student 3", "Phone_1"));
            students.Add(Student.CreateNew("Test student 3", "Phone_2"));

            var filtered_students = Student.FilterByName("Test student 1");
            Assert.AreEqual(1, filtered_students.Count);
            Assert.AreEqual(students[0], filtered_students[0]);
        }
        [TestCase]
        public void FilterByName_UsePartialNameWithOneEntry_GettingOneObject()
        {
            List<Student> students = new List<Student>();

            students.Add(Student.CreateNew("Test student 1"));
            students.Add(Student.CreateNew("Test student 4"));

            students.Add(Student.CreateNew("Test student 3", "Phone_1"));
            students.Add(Student.CreateNew("Test student 3", "Phone_2"));

            var filtered_students = Student.FilterByName("t 1");

            Assert.AreEqual(1, filtered_students.Count);
            Assert.AreEqual(students[0], filtered_students[0]);
        }
        [TestCase]
        public void FilterByName_UseLowercaseFullNameWithOneEntry_GettingOneObject()
        {
            List<Student> allStudents = new List<Student>();

            allStudents.Add(Student.CreateNew("Test student 1"));
            allStudents.Add(Student.CreateNew("Test student 4"));

            allStudents.Add(Student.CreateNew("Test student 3", "Phone_1"));
            allStudents.Add(Student.CreateNew("Test student 3", "Phone_2"));

            var students = Student.FilterByName("test student 1");
            Assert.AreEqual(1, students.Count);
            Assert.AreEqual(allStudents[0], students[0]);
        }
        [TestCase]
        public void FilterByName_UseLowercasePartialNameWithOneEntry_GettingOneObject()
        {
            List<Student> allStudents = new List<Student>();

            allStudents.Add(Student.CreateNew("Ivanov Ivan Ivanych"));
            allStudents.Add(Student.CreateNew("Test student 4"));

            allStudents.Add(Student.CreateNew("Test student 3", "Phone_1"));
            allStudents.Add(Student.CreateNew("Test student 3", "Phone_2"));

            var students = Student.FilterByName("ivanov");
            Assert.AreEqual(1, students.Count);
            Assert.AreEqual(allStudents[0], students[0]);
        }
        [TestCase]
        public void FilterByName_UseFullNameWithTwoEntry_GettingTwoObject()
        {
            List<Student> allStudents = new List<Student>();

            allStudents.Add(Student.CreateNew("Ivanov Ivan Ivanych"));
            allStudents.Add(Student.CreateNew("Test student 4"));

            allStudents.Add(Student.CreateNew("Test student 3", "Phone_1"));
            allStudents.Add(Student.CreateNew("Test student 3", "Phone_2"));

            var students = Student.FilterByName("Test student 3");
            Assert.AreEqual(2, students.Count);
            Assert.IsTrue(students.Contains(allStudents[2]));
            Assert.IsTrue(students.Contains(allStudents[3]));
        }
        [TestCase]
        public void FilterByName_UsePartialNameWithThreeEntry_GettingThreeObject()
        {
            List<Student> allStudents = new List<Student>();

            allStudents.Add(Student.CreateNew("Ivanov Ivan Ivanych"));
            allStudents.Add(Student.CreateNew("Test student 4"));

            allStudents.Add(Student.CreateNew("Test student 3", "Phone_1"));
            allStudents.Add(Student.CreateNew("Test student 3", "Phone_2"));

            var students = Student.FilterByName("Test");
            Assert.AreEqual(3, students.Count);
            Assert.IsTrue(students.Contains(allStudents[1]));
            Assert.IsTrue(students.Contains(allStudents[2]));
            Assert.IsTrue(students.Contains(allStudents[3]));
        }

        [TestCase]
        public static void Clear_ClearEmptuStudents_StudentsCountStillZero()
        {
            Student.Clear();
            Assert.AreEqual(0, Student.Count);
        }
    }
}
