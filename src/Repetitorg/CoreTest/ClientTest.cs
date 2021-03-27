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
            client.MakePayment(
                Payment.CreateNew(new DateTime(2020, 10, 10), 100000, PaymentDocumentType.PaymentOrder, 123)
            );
            Assert.AreEqual(100000, client.BalanceInKopeks);
            client.MakePayment(
                Payment.CreateNew(new DateTime(2020, 10, 15), 200000, PaymentDocumentType.PaymentOrder, 125)
            );
            Assert.AreEqual(300000, client.BalanceInKopeks);
        }
        [TestCase]
        public void Payments_MakeThreePayments_CountEqualsThree()
        {
            var client = CreateClient();
            var payments = CreatePayments();
            foreach (var p in payments)
                client.MakePayment(p);

            Assert.AreEqual(payments.Count, client.Payments.Count);
        }
        [TestCase]
        public void Payments_MakeThreePayments_PaymentsContainsAll()
        {
            var client = CreateClient();
            var payments = CreatePayments();
            foreach (var p in payments)
                client.MakePayment(p);

            EqualsOfTwoCollections(payments, client.Payments);
        }
        [TestCase]
        public void GetPaymentsLater_ExistTwoPaymentsLater_ReturnBoth()
        {
            var client = CreateClient();
            var payments = CreatePayments();
            foreach (var p in payments)
                client.MakePayment(p);
            var filteredPayments = client.GetPaymentsLater(new DateTime(2020, 12, 31));
            Assert.IsTrue(filteredPayments.Contains(payments[3]));
            Assert.IsTrue(filteredPayments.Contains(payments[4]));
            Assert.AreEqual(2, filteredPayments.Count);
        }

        private List<Payment> CreatePayments()
        {
            var payments = new List<Payment>();
            payments.Add(
                Payment.CreateNew(
                    new DateTime(2020, 10, 10), 100000, PaymentDocumentType.PaymentOrder, 123
                )
            );
            payments.Add(
                Payment.CreateNew(
                    new DateTime(2020, 10, 15), 200000, PaymentDocumentType.PaymentOrder, 125
                )
            );
            payments.Add(
                Payment.CreateNew(
                    new DateTime(2020, 10, 21), 300000, PaymentDocumentType.PaymentOrder, 127
                )
            );
            payments.Add(
                Payment.CreateNew(
                    new DateTime(2021, 1, 12), 150000, PaymentDocumentType.PaymentOrder, 129
                )
            );
            payments.Add(
                Payment.CreateNew(
                    new DateTime(2021, 2, 15), 250000, PaymentDocumentType.PaymentOrder, 131
                )
            );
            return payments;
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
        private void EqualsOfTwoCollections<T>(IEnumerable<T> first, IEnumerable<T> second)
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
