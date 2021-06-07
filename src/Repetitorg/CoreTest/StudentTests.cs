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
        public static void CreateNew_CreateStudentWithCorrectArgs_StudentsCountIncrease()
        {
            Assert.AreEqual(0, Student.StudentsCount);
            Student.CreateNew("Test Student");
            Assert.AreEqual(1, Student.StudentsCount);
        }

        [TestCase]
        public static void Clear_ClearEmptuStudents_StudentsCountStillZero()
        {
            Student.Clear();
            Assert.AreEqual(0, Student.StudentsCount);
        }
    }
}
