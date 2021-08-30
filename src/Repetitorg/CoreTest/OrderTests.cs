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
            Order.CreateNew(Client.GetAll()[0]);
            Assert.AreEqual(1, Order.Count);
        }
        [TestCase]
        public void CreateNew_CreateThreeOrder_OrdersCountEqualsThree()
        {
            var clients = Client.GetAll();
            Order.CreateNew(clients[0]);
            Order.CreateNew(clients[1]);
            Order.CreateNew(clients[2]);
            Assert.AreEqual(3, Order.Count);
        }
        [TestCase]
        public void CreateNew_CreateTwoOrderWithSameClient_OrdersCountEqualsTwo()
        {
            var clients = Client.GetAll();
            Order.CreateNew(clients[0]);
            Order.CreateNew(clients[0]);
            Assert.AreEqual(2, Order.Count);
        }
        [TestCase]
        public void CreateNew_CreateOrderWithNullClient_ThrowsException()
        {
            var exception =
                Assert.Throws<ArgumentException>(() => Order.CreateNew(null));
            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can not create order with null client"
            ));
        }
    }
}
