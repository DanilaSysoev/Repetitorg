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
        DummyPersonStorage<Student> students;
        DummyPersonStorage<Client> clients;
        DummyPaymentStorage payments;

        FullName testStudent;
        FullName testStudent1;
        FullName testStudent2;
        FullName testStudent3;
        FullName testStudent4;
        FullName ivanovII;
        FullName c1;

        PhoneNumber phoneNumber1;
        PhoneNumber phoneNumber2;
        PhoneNumber phoneNumber3;
        PhoneNumber phoneNumber4;
        PhoneNumber phoneNumber5;

        [SetUp]
        public void Initialize()
        {
            students = new DummyPersonStorage<Student>();
            clients = new DummyPersonStorage<Client>();
            payments = new DummyPaymentStorage();
            Student.SetupStorage(students);
            Client.SetupStorage(clients);

            testStudent = new FullName
            (
                firstName: "Student",
                lastName: "Test",
                patronymic: ""
            );
            testStudent1 = new FullName
            (
                firstName: "Student",
                lastName: "Test",
                patronymic: "1"
            );
            testStudent2 = new FullName
            (
                firstName: "Student",
                lastName: "Test",
                patronymic: "2"
            );
            testStudent3 = new FullName
            (
                firstName: "Student",
                lastName: "Test",
                patronymic: "3"
            );
            testStudent4 = new FullName
            (
                firstName: "Student",
                lastName: "Test",
                patronymic: "4"
            );
            ivanovII = new FullName
            (
                firstName: "Иван",
                lastName: "Иванов",
                patronymic: "Иванович"
            );
            c1 = new FullName
            (
                firstName: "",
                lastName: "c1",
                patronymic: ""
            );

            phoneNumber1 = new PhoneNumber
            (
                countryCode: 7,
                operatorCode: 900,
                number: 1112233
            );
            phoneNumber2 = new PhoneNumber
            (
                countryCode: 7,
                operatorCode: 999,
                number: 1234567
            );
            phoneNumber3 = new PhoneNumber
            (
                countryCode: 7,
                operatorCode: 800,
                number: 0000000
            );
            phoneNumber4 = new PhoneNumber
            (
                countryCode: 1,
                operatorCode: 234,
                number: 5678901
            );
            phoneNumber5 = new PhoneNumber
            (
                countryCode: 2,
                operatorCode: 345,
                number: 6789012
            );

        }

        [TestCase]
        public void StudentsCount_EmptyStudents_StudentsCountIsZero()
        {
            Assert.AreEqual(0, Student.Count);
        }

        [TestCase]
        public void CreateNew_CreateStudentWithOnlyName_StudentsCountIncrease()
        {
            Client client = Client.CreateNew(c1);
            Student.CreateNew(testStudent, client);
            Assert.AreEqual(1, Student.Count);
        }
        [TestCase]
        public void CreateNew_CreateStudentWithOnlyName_StudentAddToStorage()
        {
            Client client = Client.CreateNew(c1);
            int pastAC = students.AddCount;
            Student.CreateNew(testStudent, client);
            Assert.AreEqual(pastAC + 1, students.AddCount);
        }
        [TestCase]
        public void CreateNew_CreateStudentWithOnlyName_PhoneNumberIsEmpty()
        {
            Client client = Client.CreateNew(c1);
            Student s = Student.CreateNew(testStudent, client);

            Assert.AreEqual(null, s.PersonData.PhoneNumber);
        }
        [TestCase]
        public void CreateNew_CreateStudentWithNameAndPhoneNumber_PhoneNumberSetCorrectly()
        {
            Client client = Client.CreateNew(c1);
            Student s = Student.CreateNew(testStudent, client, phoneNumber1);

            Assert.AreEqual(phoneNumber1, s.PersonData.PhoneNumber);
        }
        [TestCase]
        public void CreateNew_CreateStudent_ClientPropertyIsOk()
        {
            Client client = Client.CreateNew(c1);
            Student s = Student.CreateNew(testStudent, client, phoneNumber1);

            Assert.AreEqual(client, s.Client);
        }
        [TestCase]
        public void CreateNew_CreateStudentWithNullName_ThrowsException()
        {
            Client client = Client.CreateNew(c1);

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
                () => Student.CreateNew(testStudent1, null)
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can not create student with null client"
            ));
        }
        [TestCase]
        public void CreateNew_CreateStudentsWithSameNameAndPhoneNumber_ThrowsException()
        {
            Client client = Client.CreateNew(c1);
            Student.CreateNew(testStudent, client, phoneNumber3);

            var exception = Assert.Throws<InvalidOperationException>(
                () => Student.CreateNew(testStudent, client, phoneNumber3)
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "creation student with same names and phone numbers is impossible"
            ));
        }

        [TestCase]
        public void GetAll_CreateTwo_AllReturned()
        {
            Client client = Client.CreateNew(c1);

            Student s1 = Student.CreateNew(testStudent1, client);
            Student s2 = Student.CreateNew(testStudent2, client);

            IReadOnlyList<Student> students = Student.GetAll();

            Assert.AreEqual(2, students.Count);
            Assert.IsTrue(students.Contains(s1));
            Assert.IsTrue(students.Contains(s2));
        }
        [TestCase]
        public void GetAll_CreateTwo_ReturnedCopyOfCollection()
        {
            Client client = Client.CreateNew(c1);

            Student s1 = Student.CreateNew(testStudent1, client);
            Student s2 = Student.CreateNew(testStudent2, client);

            IList<Student> students_old = new List<Student>(Student.GetAll());
            students_old.Remove(s1);
            IReadOnlyList<Student> students = Student.GetAll();

            Assert.AreEqual(2, students.Count);
            Assert.AreEqual(1, students_old.Count);
        }

        [TestCase]
        public void Equals_DifferentObjectsWithSameNameAndDifferentPhonNumbers_IsDifferent()
        {
            Client client = Client.CreateNew(c1);

            Student s1 = Student.CreateNew(ivanovII, client, phoneNumber1);
            Student s2 = Student.CreateNew(ivanovII, client, phoneNumber2);
            Assert.IsFalse(s1.Equals(s2));
        }
        [TestCase]
        public void Equals_EqualsWithClientWitSameNameAndPhoneNumber_IsDifferent()
        {
            Client client = Client.CreateNew(c1);

            Student s = Student.CreateNew(ivanovII, client, phoneNumber1);
            Client c = Client.CreateNew(ivanovII, phoneNumber1);
            Assert.IsFalse(s.Equals(c));
        }

        [TestCase]
        public void FilterByName_UseFullNameWithOneEntry_GettingOneObject()
        {
            List<Student> students = new List<Student>();
            Client client = Client.CreateNew(c1);

            students.Add(Student.CreateNew(testStudent1, client));
            students.Add(Student.CreateNew(testStudent4, client));

            students.Add(Student.CreateNew(testStudent3, client, phoneNumber4));
            students.Add(Student.CreateNew(testStudent3, client, phoneNumber5));

            var filtered_students = Student.FilterByName("Test student 1");
            Assert.AreEqual(1, filtered_students.Count);
            Assert.AreEqual(students[0], filtered_students[0]);
        }
        [TestCase]
        public void FilterByName_UsePartialNameWithOneEntry_GettingOneObject()
        {
            List<Student> students = new List<Student>();
            Client client = Client.CreateNew(c1);

            students.Add(Student.CreateNew(testStudent1, client));
            students.Add(Student.CreateNew(testStudent4, client));

            students.Add(Student.CreateNew(testStudent3, client, phoneNumber4));
            students.Add(Student.CreateNew(testStudent3, client, phoneNumber5));

            var filtered_students = Student.FilterByName("t 1");

            Assert.AreEqual(1, filtered_students.Count);
            Assert.AreEqual(students[0], filtered_students[0]);
        }
        [TestCase]
        public void FilterByName_UseLowercaseFullNameWithOneEntry_GettingOneObject()
        {
            List<Student> allStudents = new List<Student>();
            Client client = Client.CreateNew(c1);

            allStudents.Add(Student.CreateNew(testStudent1, client));
            allStudents.Add(Student.CreateNew(testStudent4, client));

            allStudents.Add(Student.CreateNew(testStudent3, client, phoneNumber4));
            allStudents.Add(Student.CreateNew(testStudent3, client, phoneNumber5));

            var students = Student.FilterByName("test student 1");
            Assert.AreEqual(1, students.Count);
            Assert.AreEqual(allStudents[0], students[0]);
        }
        [TestCase]
        public void FilterByName_UseLowercasePartialNameWithOneEntry_GettingOneObject()
        {
            List<Student> allStudents = new List<Student>();
            Client client = Client.CreateNew(c1);

            allStudents.Add(Student.CreateNew(ivanovII, client));
            allStudents.Add(Student.CreateNew(testStudent4, client));

            allStudents.Add(Student.CreateNew(testStudent3, client, phoneNumber4));
            allStudents.Add(Student.CreateNew(testStudent3, client, phoneNumber5));

            var students = Student.FilterByName("иванов");
            Assert.AreEqual(1, students.Count);
            Assert.AreEqual(allStudents[0], students[0]);
        }
        [TestCase]
        public void FilterByName_UseFullNameWithTwoEntry_GettingTwoObject()
        {
            List<Student> allStudents = new List<Student>();
            Client client = Client.CreateNew(c1);

            allStudents.Add(Student.CreateNew(ivanovII, client));
            allStudents.Add(Student.CreateNew(testStudent4, client));

            allStudents.Add(Student.CreateNew(testStudent3, client, phoneNumber4));
            allStudents.Add(Student.CreateNew(testStudent3, client, phoneNumber5));

            var students = Student.FilterByName("Test student 3");
            Assert.AreEqual(2, students.Count);
            Assert.IsTrue(students.Contains(allStudents[2]));
            Assert.IsTrue(students.Contains(allStudents[3]));
        }
        [TestCase]
        public void FilterByName_UsePartialNameWithThreeEntry_GettingThreeObject()
        {
            List<Student> allStudents = new List<Student>();
            Client client = Client.CreateNew(c1);

            allStudents.Add(Student.CreateNew(ivanovII, client));
            allStudents.Add(Student.CreateNew(testStudent4, client));

            allStudents.Add(Student.CreateNew(testStudent3, client, phoneNumber4));
            allStudents.Add(Student.CreateNew(testStudent3, client, phoneNumber5));

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
            Client client = Client.CreateNew(c1);

            allStudents.Add(Student.CreateNew(ivanovII, client));
            allStudents.Add(Student.CreateNew(testStudent4, client));

            allStudents.Add(Student.CreateNew(testStudent3, client, phoneNumber4));
            allStudents.Add(Student.CreateNew(testStudent3, client, phoneNumber5));

            var students = Student.FilterByName("test");
            Assert.AreEqual(3, students.Count);
            Assert.IsTrue(students.Contains(allStudents[1]));
            Assert.IsTrue(students.Contains(allStudents[2]));
            Assert.IsTrue(students.Contains(allStudents[3]));
        }
        [TestCase]
        public void FilterByName_filterByNull_ThrowsException()
        {
            Client client = Client.CreateNew(c1);

            Student.CreateNew(ivanovII, client);
            Student.CreateNew(testStudent4, client);
            Student.CreateNew(testStudent3, client, phoneNumber4);
            Student.CreateNew(testStudent3, client, phoneNumber5);

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
            Client client = Client.CreateNew(c1);
            Student s = Student.CreateNew(ivanovII, client);
            Assert.IsTrue(s.ToString().Contains("Иванов Иван Иванович"));
        }
    }
}
