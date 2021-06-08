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
            Assert.AreEqual(0, Student.StudentsCount);
        }

        [TestCase]
        public static void CreateNew_CreateStudentWithOnlyName_StudentsCountIncrease()
        {
            Student.CreateNew("Test Student");
            Assert.AreEqual(1, Student.StudentsCount);
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

            Assert.IsTrue(exception.Message.Contains(
                "Name of the student can't be null"
            ));
        }
        [TestCase]
        public static void CreateNew_CreateStudentWithNullPhoneNumber_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => Student.CreateNew("Test student", null)
            );

            Assert.IsTrue(exception.Message.Contains(
                "Phone number of the student can't be null"
            ));
        }
        [TestCase]
        public static void CreateNew_CreateStudentsWithSameNameAndPhoneNumber_ThrowsException()
        {
            Student.CreateNew("Test student", "Test Phone");

            var exception = Assert.Throws<InvalidOperationException>(
                () => Student.CreateNew("Test student", "Test Phone")
            );

            Assert.IsTrue(exception.Message.Contains(
                "Creation student with same names and phone numbers is impossible"
            ));
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
        public static void Clear_ClearEmptuStudents_StudentsCountStillZero()
        {
            Student.Clear();
            Assert.AreEqual(0, Student.StudentsCount);
        }
    }
}
