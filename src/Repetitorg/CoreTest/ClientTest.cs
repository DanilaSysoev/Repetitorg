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
        [TearDown]
        public void Clear()
        {
            Client.Clear();
        }

        [TestCase]
        public void CreateNew_CreateTwoClients_IncrementClientsCount()
        {
            CreateClients();
            Assert.AreEqual(2, Client.ClientsCount);
        }
        [TestCase]
        public void ClientsCount_EmptyClients_EqualZero()
        {
            Assert.AreEqual(0, Client.ClientsCount);
        }
        [TestCase]
        public void All_CreateTwoClients_AllExist()
        {
            var clients = CreateClients();
            foreach(var client in Client.All)
            {
                Assert.IsTrue(clients.Contains(client));
            }
            foreach(var client in clients)
            {
                Assert.IsTrue(Client.All.Contains(client));
            }
        }
        [TestCase]
        public void Equals_differentObjectsWithSameName_IsDifferent()
        {
            Client c1 = Client.CreateNew("Иванов Иван Иванович");
            Client c2 = Client.CreateNew("Иванов Иван Иванович");
            Assert.IsFalse(c1.Equals(c2));
        }

        private List<Client> CreateClients()
        {
            List<Client> clients = new List<Client>();

            clients.Add(Client.CreateNew("Иванов Иван Иванович"));
            clients.Add(Client.CreateNew("Петров Петр Петрович"));

            return clients;
        }
    }
}
