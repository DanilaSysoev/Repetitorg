using NUnit.Framework;
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
        [TestCase]
        public static void Create_CreateStudentWithCorrectArgs_StudentsCountIncrease()
        {
            Assert.AreEqual(0, Student.StudentsCount);
            Student.Create("Test Student");
            Assert.AreEqual(1, Student.StudentsCount);
        }
    }
}
