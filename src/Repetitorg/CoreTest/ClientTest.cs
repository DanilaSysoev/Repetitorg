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
        public void CreateNew_CreateFourClients_IncrementClientsCount()
        {
            CreateClients();
            Assert.AreEqual(4, Client.ClientsCount);
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
            EqualsOfTwoCollections(clients, Client.All);
        }

        [TestCase]
        public void Equals_differentObjectsWithSameName_IsDifferent()
        {
            Client c1 = Client.CreateNew("Иванов Иван Иванович");
            Client c2 = Client.CreateNew("Иванов Иван Иванович");
            Assert.IsFalse(c1.Equals(c2));
        }
        [TestCase]
        public void FilterByName_UseFullNameWithOneEntry_GettingOneObject()
        {
            var allClients = CreateClients();
            var clients = Client.FilterByName("Иванов Иван Иванович");
            Assert.AreEqual(1, clients.Count);
            Assert.AreEqual(allClients[0], clients[0]);
        }
        [TestCase]
        public void FilterByName_UsePartialNameWithOneEntry_GettingOneObject()
        {
            var allClients = CreateClients();
            var clients = Client.FilterByName("Иванов");
            Assert.AreEqual(1, clients.Count);
            Assert.AreEqual(allClients[0], clients[0]);
        }
        [TestCase]
        public void FilterByName_UseLowercaseFullNameWithOneEntry_GettingOneObject()
        {
            var allClients = CreateClients();
            var clients = Client.FilterByName("иванов иван иванович");
            Assert.AreEqual(1, clients.Count);
            Assert.AreEqual(allClients[0], clients[0]);
        }
        [TestCase]
        public void FilterByName_UseLowercasePartialNameWithOneEntry_GettingOneObject()
        {
            var allClients = CreateClients();
            var clients = Client.FilterByName("Иванов");
            Assert.AreEqual(1, clients.Count);
            Assert.AreEqual(allClients[0], clients[0]);
        }
        [TestCase]
        public void FilterByName_UseFullNameWithTwoEntry_GettingTwoObject()
        {
            var allClients = CreateClients();
            var clients = Client.FilterByName("Петров Петр Петрович");
            Assert.AreEqual(2, clients.Count);
            Assert.IsTrue(clients.Contains(allClients[1]));
            Assert.IsTrue(clients.Contains(allClients[3]));
        }
        [TestCase]
        public void FilterByName_UsePartialNameWithThreeEntry_GettingThreeObject()
        {
            var allClients = CreateClients();
            var clients = Client.FilterByName("Петров");
            Assert.AreEqual(3, clients.Count);
            Assert.IsTrue(clients.Contains(allClients[1]));
            Assert.IsTrue(clients.Contains(allClients[2]));
            Assert.IsTrue(clients.Contains(allClients[3]));
        }
        [TestCase]
        public void FilterByName_UseLowercasePartialNameWithThreeEntry_GettingThreeObject()
        {
            var allClients = CreateClients();
            var clients = Client.FilterByName("петров");
            Assert.AreEqual(3, clients.Count);
            Assert.IsTrue(clients.Contains(allClients[1]));
            Assert.IsTrue(clients.Contains(allClients[2]));
            Assert.IsTrue(clients.Contains(allClients[3]));
        }
        [TestCase]
        public void ToString_SimpleClient_ContainsfullName()
        {
            var client = CreateClient();
            Assert.IsTrue(client.ToString().Contains("Иванов Иван Иванович"));
        }
        [TestCase]
        public void Balance_NewClient_BalanceZero()
        {
            var client = CreateClient();
            Assert.AreEqual(0, client.BalanceInKopeks);
        }
        [TestCase]
        public void MakePayment_ClientMakesPayment_BalanceIncreases()
        {
            var client = CreateClient();
            client.MakePayment(new DateTime(2020, 10, 10), 100000);
            Assert.AreEqual(100000, client.BalanceInKopeks);
            client.MakePayment(new DateTime(2020, 10, 15), 200000);
            Assert.AreEqual(300000, client.BalanceInKopeks);
        }


        private Client CreateClient()
        {
            var c = Client.CreateNew("Иванов Иван Иванович");
            return c;
        }
        private List<Client> CreateClients()
        {
            List<Client> clients = new List<Client>();

            clients.Add(Client.CreateNew("Иванов Иван Иванович"));
            clients.Add(Client.CreateNew("Петров Петр Петрович"));
            clients.Add(Client.CreateNew("Петровa Aнфстасия Владимировна"));
            clients.Add(Client.CreateNew("Петров Петр Петрович"));

            return clients;
        }
        private void EqualsOfTwoCollections(IEnumerable<Client> first, IEnumerable<Client> second)
        {
            foreach (var client in second)
            {
                Assert.IsTrue(first.Contains(client));
            }
            foreach (var client in first)
            {
                Assert.IsTrue(second.Contains(client));
            }
        }
    }
}
