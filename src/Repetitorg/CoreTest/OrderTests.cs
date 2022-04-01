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
    class OrderTests
    {
        DummyPersonStorage<Student> students;
        DummyPersonStorage<Client> clients;
        DummyPaymentStorage payments;
        DummyOrderStorage orders;

        [SetUp]
        public void Initialize()
        {
            students = new DummyPersonStorage<Student>();
            clients = new DummyPersonStorage<Client>();
            payments = new DummyPaymentStorage();
            orders = new DummyOrderStorage();

            Student.InitializeStorage(students);
            Client.InitializeStorage(clients);
            Payment.InitializeStorage(payments);
            Order.InitializeStorage(orders);

            Client c1 = Client.CreateNew("c1");
            Client c2 = Client.CreateNew("c2");
            Client c3 = Client.CreateNew("c3");

            Student.CreateNew("s1", c1);
            Student.CreateNew("s2", c2);
            Student.CreateNew("s3", c3);
        }

        [TestCase]
        public void CreateNew_CreateOneOrder_OrdersCountEqualsOne()
        {
            Order.CreateNew("o1");
            Assert.AreEqual(1, Order.Count);
        }
        [TestCase]
        public void CreateNew_CreateThreeOrder_OrdersCountEqualsThree()
        {
            var clients = Client.GetAll();
            Order.CreateNew("o1");
            Order.CreateNew("o2");
            Order.CreateNew("o3");
            Assert.AreEqual(3, Order.Count);
        }
        [TestCase]
        public void CreateNew_CreateTwoOrderWithSameClientAndDifferenceNames_OrdersCountEqualsTwo()
        {
            var clients = Client.GetAll();
            Order.CreateNew("o1");
            Order.CreateNew("o2");
            Assert.AreEqual(2, Order.Count);
        }
        [TestCase]
        public void CreateNew_CreateTwoOrderWithSameClientAndSameNames_ThrowsException()
        {
            var clients = Client.GetAll();
            Order.CreateNew("o1");
            var exception = Assert.Throws<InvalidOperationException>(
                () => Order.CreateNew("o1")
            );
            Assert.IsTrue(exception.Message.ToLower().Contains(
                "order with given name already exist"
            ));
        }
        [TestCase]
        public void CreateNew_CreateOrderWithNullName_ThrowsException()
        {
            var exception =
                Assert.Throws<ArgumentException>(
                    () => Order.CreateNew(null)
                );
            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can not create order with null name"
            ));
        }
        [TestCase]
        public void CreateNew_CreateOrderWithSomeName_NamePropertyIsCorrect()
        {
            var clients = Client.GetAll();
            Order order = Order.CreateNew("o1");
            Assert.AreEqual("o1", order.Name);
        }

        [TestCase]
        public void Equals_CreateTwoEqualsOrdersInDifferentSeesions_EqualsReturnTrue()
        {
            var clients = Client.GetAll();
            Order order1 = Order.CreateNew("o1");
            Order.InitializeStorage(new DummyOrderStorage());
            Order order2 = Order.CreateNew("o1");
            Assert.IsTrue(order1.Equals(order2));
        }
        [TestCase]
        public void Equals_CreateTwoOrdersWithDifferentNames_EqualsReturnFalse()
        {
            var clients = Client.GetAll();
            Order order1 = Order.CreateNew("o1");
            Order order2 = Order.CreateNew("o2");
            Assert.IsFalse(order1.Equals(order2));
        }

        [TestCase]
        public void ToString_CreateOrder_ToStringReturnCorrectRepresentation()
        {
            var clients = Client.GetAll();
            Order order = Order.CreateNew("o1");
            Assert.AreEqual("o1", order.ToString());
        }
        
        [TestCase]
        public void StudentsCount_CreateOrder_StudentsCountEqualsZero()
        {
            Order order = Order.CreateNew("o1");
            Assert.AreEqual(0, order.Students.Count);
        }

        [TestCase]
        public void AddStudent_AddOneStudent_StudentsCountEqualsOne()
        {
            Order order = Order.CreateNew("o1");
            order.AddStudent(Student.GetAll()[0], 100000);

            Assert.AreEqual(1, order.Students.Count);
        }
        [TestCase]
        public void AddStudent_AddNullStudent_ThrowsException()
        {
            Order order = Order.CreateNew("o1");
            var exception = Assert.Throws<ArgumentException>(
                () => order.AddStudent(null, 100000)
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can not add null student to order"
            ));
        }
        [TestCase]
        public void AddStudent_AddAlreadyAddedStudent_ThrowsException()
        {
            Order order = Order.CreateNew("o1");
            var students = Student.GetAll();
            order.AddStudent(students[0], 100000);
            var exception = Assert.Throws<ArgumentException>(
                () => order.AddStudent(students[0], 100000)
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "student already added"
            ));
        }
        [TestCase]
        public void AddStudent_AddWithZeroCost_StudentsCountEqualsOne()
        {
            Order order = Order.CreateNew("o1");
            order.AddStudent(Student.GetAll()[0], 0);

            Assert.AreEqual(1, order.Students.Count);
        }
        [TestCase]
        public void AddStudent_AddWithNegativeCost_ThrowsException()
        {
            Order order = Order.CreateNew("o1");
            var exception = Assert.Throws<ArgumentException>(
                () => order.AddStudent(Student.GetAll()[0], -1)
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can not add student with negative cost to order"
            ));
        }
        [TestCase]
        public void AddStudent_AddThreeStudents_StudentsCountEqualsThree()
        {
            Order order = Order.CreateNew("o1");
            var students = Student.GetAll();
            order.AddStudent(students[0], 100000);
            order.AddStudent(students[1], 100000);
            order.AddStudent(students[2], 100000);

            Assert.AreEqual(3, order.Students.Count);
        }
        [TestCase]
        public void AddStudent_AddThreeStudents_StudentsContainsAll()
        {
            Order order = Order.CreateNew("o1");
            var students = Student.GetAll();
            order.AddStudent(students[0], 100000);
            order.AddStudent(students[1], 100000);
            order.AddStudent(students[2], 100000);

            Assert.IsTrue(order.Students.Contains(students[0]));
            Assert.IsTrue(order.Students.Contains(students[1]));
            Assert.IsTrue(order.Students.Contains(students[2]));
        }

        [TestCase]
        public void RemoveStudent_RemoveExistingStudent_StudentsCountDecrease()
        {
            Order order = Order.CreateNew("o1");
            var students = Student.GetAll();
            order.AddStudent(students[0], 100000);
            order.AddStudent(students[1], 100000);
            order.AddStudent(students[2], 100000);

            order.RemoveStudent(students[0]);
            Assert.AreEqual(2, order.Students.Count);
        }
        [TestCase]
        public void RemoveStudent_RemoveExistingStudent_RestExist()
        {
            Order order = Order.CreateNew("o1");
            var students = Student.GetAll();
            order.AddStudent(students[0], 100000);
            order.AddStudent(students[1], 100000);
            order.AddStudent(students[2], 100000);

            order.RemoveStudent(students[0]);
            Assert.IsTrue(order.Students.Contains(students[1]));
            Assert.IsTrue(order.Students.Contains(students[2]));
        }
        [TestCase]
        public void RemoveStudent_RemoveNonExistentStudent_ThrowsException()
        {
            Order order = Order.CreateNew("o1");
            var students = Student.GetAll();
            order.AddStudent(students[0], 100000);

            var exception = Assert.Throws<ArgumentException>(
                () => order.RemoveStudent(students[1])
            );
            Assert.IsTrue(exception.Message.ToLower().Contains(
                "student is not in order"
            ));
        }
        [TestCase]
        public void RemoveStudent_RemoveNullStudent_ThrowsException()
        {
            Order order = Order.CreateNew("o1");
            
            var exception = Assert.Throws<ArgumentException>(
                () => order.RemoveStudent(null)
            );
            Assert.IsTrue(exception.Message.ToLower().Contains(
                "student can not be null"
            ));
        }

        [TestCase]
        public void GetAll_NewStorage_ReturnEmpty()
        {
            Assert.AreEqual(0, Order.Count);
        }
        [TestCase]
        public void GetAll_AddThreeOrders_ReturnCountEqualsThree()
        {
            Order.CreateNew("o1");
            Order.CreateNew("o2");
            Order.CreateNew("o3");
            Assert.AreEqual(3, Order.Count);
        }
    }
}
