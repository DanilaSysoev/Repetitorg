using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repetitorg.Core;

namespace CoreTest
{
    [TestFixture]
    class ClientTest
    {
        [SetUp]
        public void Initialize()
        {
            Client.Initialize();
        }
        [TearDown]
        public void Clear()
        {
            Client.Clear();
        }

        [TestCase]
        public void CreateNew_CreateTwoClients_IncrementClientsCount()
        {
            Client.CreateNew("Иванов Иван Иванович");
            Client.CreateNew("Петров Петр Петрович");
            Assert.AreEqual(2, Client.ClientsCount);
        }
    }
}
