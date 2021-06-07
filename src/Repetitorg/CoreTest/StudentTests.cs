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
        public static void Clear_ClearEmptuStudents_StudentsCountStillZero()
        {
            Student.Clear();
            Assert.AreEqual(0, Student.StudentsCount);
        }
    }
}
