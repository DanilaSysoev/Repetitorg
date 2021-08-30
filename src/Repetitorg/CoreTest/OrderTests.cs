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
        [TearDown]
        public void Clear()
        {
            Order.Clear();
            Client.Clear();
            Student.Clear();
        }
        [SetUp]
        public void Initialize()
        {
            Client.CreateNew("c1");
            Client.CreateNew("c2");
            Client.CreateNew("c3");

            Student.CreateNew("s1");
            Student.CreateNew("s2");
            Student.CreateNew("s3");
        }

        [TestCase]
        public void CreateNew_CreateOneOrder_OrdersCountEqualsOne()
        {
            Order.CreateNew("o1", Client.GetAll()[0]);
            Assert.AreEqual(1, Order.Count);
        }
        [TestCase]
        public void CreateNew_CreateThreeOrder_OrdersCountEqualsThree()
        {
            var clients = Client.GetAll();
            Order.CreateNew("o1", clients[0]);
            Order.CreateNew("o2", clients[1]);
            Order.CreateNew("o3", clients[2]);
            Assert.AreEqual(3, Order.Count);
        }
        [TestCase]
        public void CreateNew_CreateTwoOrderWithSameClientAndDifferenceNames_OrdersCountEqualsTwo()
        {
            var clients = Client.GetAll();
            Order.CreateNew("o1", clients[0]);
            Order.CreateNew("o2", clients[0]);
            Assert.AreEqual(2, Order.Count);
        }
        [TestCase]
        public void CreateNew_CreateTwoOrderWithDifferentClientAndSameNames_OrdersCountEqualsTwo()
        {
            var clients = Client.GetAll();
            Order.CreateNew("o1", clients[0]);
            Order.CreateNew("o1", clients[1]);
            Assert.AreEqual(2, Order.Count);
        }
        [TestCase]
        public void CreateNew_CreateTwoOrderWithSameClientAndSameNames_ThrowsException()
        {
            var clients = Client.GetAll();
            Order.CreateNew("o1", clients[0]);
            var exception = Assert.Throws<InvalidOperationException>(
                () => Order.CreateNew("o1", clients[0])
            );
            Assert.IsTrue(exception.Message.ToLower().Contains(
                "order with given name and client already exist"
            ));
        }
        [TestCase]
        public void CreateNew_CreateOrderWithNullClient_ThrowsException()
        {
            var exception =
                Assert.Throws<ArgumentException>(() => Order.CreateNew("o1", null));
            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can not create order with null client"
            ));
        }
        [TestCase]
        public void CreateNew_CreateOrderWithNullName_ThrowsException()
        {
            var exception =
                Assert.Throws<ArgumentException>(
                    () => Order.CreateNew(null, Client.GetAll()[0])
                );
            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can not create order with null name"
            ));
        }
        [TestCase]
        public void CreateNew_CreateOrderWithSomeClient_ClientPropertyIsCorrect()
        {
            var clients = Client.GetAll();
            Order order = Order.CreateNew("o1", clients[0]);
            Assert.AreEqual(clients[0], order.Client);
        }
        [TestCase]
        public void CreateNew_CreateOrderWithSomeName_NamePropertyIsCorrect()
        {
            var clients = Client.GetAll();
            Order order = Order.CreateNew("o1", clients[0]);
            Assert.AreEqual("o1", order.Name);
        }

        [TestCase]
        public void Equals_CreateTwoEqualsOrdersInDifferentSeesions_EqualsReturnTrue()
        {
            var clients = Client.GetAll();
            Order order1 = Order.CreateNew("o1", clients[0]);
            Order.Clear();
            Order order2 = Order.CreateNew("o1", clients[0]);
            Assert.IsTrue(order1.Equals(order2));
        }
        [TestCase]
        public void Equals_CreateTwoOrdersWithDifferentNames_EqualsReturnFalse()
        {
            var clients = Client.GetAll();
            Order order1 = Order.CreateNew("o1", clients[0]);
            Order order2 = Order.CreateNew("o2", clients[0]);
            Assert.IsFalse(order1.Equals(order2));
        }
        [TestCase]
        public void Equals_CreateTwoOrdersWithDifferentClients_EqualsReturnFalse()
        {
            var clients = Client.GetAll();
            Order order1 = Order.CreateNew("o1", clients[0]);
            Order order2 = Order.CreateNew("o1", clients[1]);
            Assert.IsFalse(order1.Equals(order2));
        }
        [TestCase]
        public void Equals_CreateTwoOrdersWithDifferentNamesAndClients_EqualsReturnFalse()
        {
            var clients = Client.GetAll();
            Order order1 = Order.CreateNew("o1", clients[0]);
            Order order2 = Order.CreateNew("o2", clients[1]);
            Assert.IsFalse(order1.Equals(order2));
        }

        [TestCase]
        public void ToString_CreateOrder_ToStringReturnCorrectRepresentation()
        {
            var clients = Client.GetAll();
            Order order = Order.CreateNew("o1", clients[0]);
            Assert.AreEqual("c1: o1", order.ToString());
        }
        
        [TestCase]
        public void StudentsCount_CreateOrder_StudentsCountEqualsZero()
        {
            Order order = Order.CreateNew("o1", Client.GetAll()[0]);
            Assert.AreEqual(0, order.Students.Count);
        }

        [TestCase]
        public void AddStudent_AddOneStudent_StudentsCountEqualsOne()
        {
            Order order = Order.CreateNew("o1", Client.GetAll()[0]);
            order.AddStudent(Student.GetAll()[0], 100000);

            Assert.AreEqual(1, order.Students.Count);
        }
        [TestCase]
        public void AddStudent_AddNullStudent_ThrowsException()
        {
            Order order = Order.CreateNew("o1", Client.GetAll()[0]);
            var exception = Assert.Throws<ArgumentException>(
                () => order.AddStudent(null, 100000)
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can not add null student to order"
            ));
        }
        [TestCase]
        public void AddStudent_AddWithZeroCost_StudentsCountEqualsOne()
        {
            Order order = Order.CreateNew("o1", Client.GetAll()[0]);
            order.AddStudent(Student.GetAll()[0], 0);

            Assert.AreEqual(1, order.Students.Count);
        }
        [TestCase]
        public void AddStudent_AddWithNegativeCost_ThrowsException()
        {
            Order order = Order.CreateNew("o1", Client.GetAll()[0]);
            var exception = Assert.Throws<ArgumentException>(
                () => order.AddStudent(Student.GetAll()[0], -1)
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can not add student with negative cost to order"
            ));
        }
    }
}
