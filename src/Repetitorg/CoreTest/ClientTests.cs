using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repetitorg.Core;
using Repetitorg.Core.Exceptions;

namespace Repetitorg.CoreTest
{
    [TestFixture]
    class ClientTests
    {
        DummyPersonStorage<Student> students;
        DummyPersonStorage<Client> clients;
        DummyPaymentStorage payments;

        [SetUp]
        public void Initialize()
        {
            students = new DummyPersonStorage<Student>();
            clients = new DummyPersonStorage<Client>();
            payments = new DummyPaymentStorage();
            Student.InitializeStorage(students);
            Client.InitializeStorage(clients);
        }

        [TestCase]
        public void CreateNew_CreateFourClients_IncrementClientsCount()
        {
            CreateClients();
            Assert.AreEqual(4, Client.Count);
        }
        [TestCase]
        public void CreateNew_NameIsNull_ThrowsException()
        {
            var exception = 
                Assert.Throws<ArgumentException>(() => Client.CreateNew(payments, null));
            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can not create client with null name"
            ));
        }
        [TestCase]
        public void CreateNew_PhoneNumberIsNull_ThrowsException()
        {
            var exception =
                Assert.Throws<ArgumentException>(
                    () => Client.CreateNew(payments, "some name", null)
                );
            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can not create client with null phone number"
            ));
        }
        [TestCase]
        public void CreateNew_CreateTwoWithSameNameAndPhoneNumber_ThrowsException()
        {
            Client c1 = Client.CreateNew(payments, "Иванов Иван Иванович", "8-999-123-45-67");

            var exception = Assert.Throws<InvalidOperationException>(
                () => Client.CreateNew(payments, "Иванов Иван Иванович", "8-999-123-45-67")
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "creation client with same names and phone numbers is impossible"
            ));
        }

        [TestCase]
        public void ClientsCount_EmptyClients_EqualZero()
        {
            Assert.AreEqual(0, Client.Count);
        }

        [TestCase]
        public void GetAll_CreateTwo_AllReturned()
        {
            Client c1 = Client.CreateNew(payments, "Иванов Иван Иванович");
            Client c2 = Client.CreateNew(payments, "Петров Петр Петрович");

            IReadOnlyList<Client> clients = Client.GetAll();

            Assert.AreEqual(2, clients.Count);
            Assert.IsTrue(clients.Contains(c1));
            Assert.IsTrue(clients.Contains(c2));
        }
        [TestCase]
        public void GetAll_CreateTwo_ReturnedCopyOfCollection()
        {
            Client c1 = Client.CreateNew(payments, "Иванов Иван Иванович");
            Client c2 = Client.CreateNew(payments, "Петров Петр Петрович");

            IList<Client> clients_old = new List<Client>(Client.GetAll());
            clients_old.Remove(c1);
            IReadOnlyList<Client> clients = Client.GetAll();

            Assert.AreEqual(2, clients.Count);
            Assert.AreEqual(1, clients_old.Count);
        }

        [TestCase]
        public void Equals_DifferentObjectsWithSameNameAndDifferentPhonNumbers_IsDifferent()
        {
            Client c1 = Client.CreateNew(payments, "Иванов Иван Иванович", "8-999-123-45-67");
            Client c2 = Client.CreateNew(payments, "Иванов Иван Иванович", "8-999-456-78-90");
            Assert.IsFalse(c1.Equals(c2));
        }
        [TestCase]
        public void Equals_EqualsWithStudentWitSameNameAndPhoneNumber_IsDifferent()
        {
            Client c = Client.CreateNew(payments, "Иванов Иван Иванович", "8-999-123-45-67");
            Student s = Student.CreateNew("Иванов Иван Иванович", c, "8-999-123-45-67");
            Assert.IsFalse(c.Equals(s));
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
            var clients = Client.FilterByName("иванов");
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
        public void FilterByName_filterByNull_ThrowsException()
        {
            CreateClients();

            var exception = Assert.Throws<ArgumentException>(
                () => Client.FilterByName(null)
            );

            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "filtering by null pattern is impossible"
                )
            );
        }

        [TestCase]
        public void ToString_SimpleClient_ContainsFullName()
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
        public void MakePayment_NegativePayment_ThrowsException()
        {
            var client = CreateClient();
            var exception = Assert.Throws<ArgumentException>(
                () => client.MakePayment(
                    Payment.CreateNew(new DateTime(2020, 10, 10), -100000, PaymentDocumentType.PaymentOrder, 123)
                )
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "payment should has positive value"
            ));
        }
        [TestCase]
        public void MakePayment_ZeroPayment_ThrowsException()
        {
            var client = CreateClient();
            var exception = Assert.Throws<ArgumentException>(
                () => client.MakePayment(
                    Payment.CreateNew(new DateTime(2020, 10, 10), 0, PaymentDocumentType.PaymentOrder, 123)
                )
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "payment should has positive value"
            ));
        }

        [TestCase]
        public void Payments_MakeNullPayments_ThrowsException()
        {
            var client = CreateClient();

            var exception = Assert.Throws<ArgumentException>(
                () => client.MakePayment(null)
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "payment can't be null"
            ));
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
        [TestCase]
        public void GetPaymentsLater_DateEqualLastByDatePayment_ReturnEmptyBecauseExclude()
        {
            var client = CreateClient();
            var payments = CreatePayments();
            foreach (var p in payments)
                client.MakePayment(p);
            var filteredPayments = client.GetPaymentsLater(new DateTime(2021, 2, 15));
            Assert.AreEqual(0, filteredPayments.Count);
        }
        [TestCase]
        public void GetPaymentsBefore_ExistTwoPaymentsBefore_ReturnBoth()
        {
            var client = CreateClient();
            var payments = CreatePayments();
            foreach (var p in payments)
                client.MakePayment(p);
            var filteredPayments = client.GetPaymentsBefore(new DateTime(2020, 10, 17));
            Assert.IsTrue(filteredPayments.Contains(payments[0]));
            Assert.IsTrue(filteredPayments.Contains(payments[1]));
            Assert.AreEqual(2, filteredPayments.Count);
        }
        [TestCase]
        public void GetPaymentsBefore_DateEqualFirstByDatePayment_ReturnEmptyBecauseExclude()
        {
            var client = CreateClient();
            var payments = CreatePayments();
            foreach (var p in payments)
                client.MakePayment(p);
            var filteredPayments = client.GetPaymentsBefore(new DateTime(2020, 10, 10));
            Assert.AreEqual(0, filteredPayments.Count);
        }
        [TestCase]
        public void GetPaymentsBetween_ExistTwoPaymentsBetween_ReturnBoth()
        {
            var client = CreateClient();
            var payments = CreatePayments();
            foreach (var p in payments)
                client.MakePayment(p);
            var filteredPayments = client.GetPaymentsBetween(new DateTime(2020, 10, 12), new DateTime(2020, 10, 25));
            Assert.IsTrue(filteredPayments.Contains(payments[1]));
            Assert.IsTrue(filteredPayments.Contains(payments[2]));
            Assert.AreEqual(2, filteredPayments.Count);
        }
        [TestCase]
        public void GetPaymentsBetween_EndDateEqualFirstPaymentByDate_ReturnEmptyBecauseExclude()
        {
            var client = CreateClient();
            var payments = CreatePayments();
            foreach (var p in payments)
                client.MakePayment(p);
            var filteredPayments = client.GetPaymentsBetween(new DateTime(2020, 9, 5), new DateTime(2020, 10, 10));
            Assert.AreEqual(0, filteredPayments.Count);
        }
        [TestCase]
        public void GetPaymentsBetween_BeginDateEqualLastPaymentByDate_ReturnLastPaymentBecauseInclude()
        {
            var client = CreateClient();
            var payments = CreatePayments();
            foreach (var p in payments)
                client.MakePayment(p);
            var filteredPayments = client.GetPaymentsBetween(new DateTime(2021, 2, 15), new DateTime(2021, 3, 21));
            Assert.IsTrue(filteredPayments.Contains(payments[4]));
            Assert.AreEqual(1, filteredPayments.Count);
        }
        [TestCase]
        public void FullName_CreateNewClient_FullNameSetupCorrect()
        {
            var client = CreateClient();
            Assert.AreEqual("Иванов Иван Иванович", client.FullName);
        }
        [TestCase]
        public void PhoneNumber_CreateNewClientWithoutPhoneNumber_PhoneNumberIsEmpty()
        {
            var client = CreateClient();
            Assert.AreEqual("", client.PhoneNumber);
        }
        [TestCase]
        public void PhoneNumber_CreateNewClientWithPhoneNumber_PhoneNumberIsCorrect()
        {
            var client = CreateClientWithPhoneNumber();
            Assert.AreEqual("+7(900)111-22-33", client.PhoneNumber);
        }
        [TestCase]
        public void PhoneNumber_ChangePhoneNumber_PhoneNumberIsCorrect()
        {
            var client = CreateClient();
            Assert.AreEqual("", client.PhoneNumber);
            client.PhoneNumber = "+7(900)111-22-33";
            Assert.AreEqual("+7(900)111-22-33", client.PhoneNumber);
        }
        [TestCase]
        public void PhoneNumber_PhoneNumberSetNull_ThrowsException()
        {
            var client = CreateClient();
            Assert.AreEqual("", client.PhoneNumber);
            var exception = Assert.Throws<InvalidPhoneNumberException>(() => client.PhoneNumber = null);
            Assert.IsTrue(exception.Message.ToLower().Contains("phonenumber can't be null"));
        }

        private Client CreateClientWithPhoneNumber()
        {
            var c = Client.CreateNew(payments, "Иванов Иван Иванович", "+7(900)111-22-33");
            return c;
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
            var c = Client.CreateNew(payments, "Иванов Иван Иванович");
            return c;
        }
        private List<Client> CreateClients()
        {
            List<Client> clients = new List<Client>();

            clients.Add(Client.CreateNew(payments, "Иванов Иван Иванович"));
            clients.Add(Client.CreateNew(payments, "Петров Петр Петрович", "Phone_1"));
            clients.Add(Client.CreateNew(payments, "Петровa Aнастасия Владимировна"));
            clients.Add(Client.CreateNew(payments, "Петров Петр Петрович", "Phone_2"));

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
