﻿using NUnit.Framework;
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
        DummyStorage<Student> students;
        DummyStorage<Client> clients;
        DummyStorage<Payment> payments;
        DummyStorage<PaymentDocumentType> paymentDocuments;

        PaymentDocumentType paymentOrder;

        PhoneNumber phoneNumber1;
        PhoneNumber phoneNumber2;
        PhoneNumber phoneNumber3;
        PhoneNumber phoneNumber4;
        PhoneNumber phoneNumber5;
        FullName ivanovII;
        FullName petrovPP;
        FullName petrovaAI;

        [SetUp]
        public void Initialize()
        {
            students = new DummyStorage<Student>();
            clients = new DummyStorage<Client>();
            payments = new DummyStorage<Payment>();
            paymentDocuments = new DummyStorage<PaymentDocumentType>();
            Student.SetupStorage(students);
            Client.SetupStorage(clients);
            Payment.SetupStorage(payments);
            PaymentDocumentType.SetupStorage(paymentDocuments);

            paymentOrder = PaymentDocumentType.CreateNew("PaymentOrder");

            phoneNumber1 = new PhoneNumber
            (
                countryCode: "7",
                operatorCode: "900",
                number: "1112233"
            );
            phoneNumber2 = new PhoneNumber
            (
                countryCode: "7",
                operatorCode: "999",
                number: "1234567"
            );
            phoneNumber3 = new PhoneNumber
            (
                countryCode: "7",
                operatorCode: "800",
                number: "0000000"
            );
            phoneNumber4 = new PhoneNumber
            (
                countryCode: "1",
                operatorCode: "234",
                number: "5678901"
            );
            phoneNumber5 = new PhoneNumber
            (
                countryCode: "2",
                operatorCode: "345",
                number: "6789012"
            );
            ivanovII = new FullName
            (
                firstName: "Иван",
                lastName: "Иванов",
                patronymic: "Иванович"
            );
            petrovPP = new FullName
            (
                firstName: "Петр",
                lastName: "Петров",
                patronymic: "Петрович"
            );
            petrovaAI = new FullName
            (
                firstName: "Анастасия",
                lastName: "Петрова",
                patronymic: "Владимировна"
            );

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
                Assert.Throws<ArgumentException>(() => Client.CreateNew(null));
            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can not create client with null name"
            ));
        }
        [TestCase]
        public void CreateNew_CreateTwoWithSameNameAndPhoneNumber_ThrowsException()
        {
            Client c1 = Client.CreateNew(ivanovII, phoneNumber2);

            var exception = Assert.Throws<InvalidOperationException>(
                () => Client.CreateNew(ivanovII, phoneNumber2)
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
            Client c1 = Client.CreateNew(ivanovII);
            Client c2 = Client.CreateNew(petrovPP);

            IReadOnlyList<Client> clients = Client.GetAll();

            Assert.AreEqual(2, clients.Count);
            Assert.IsTrue(clients.Contains(c1));
            Assert.IsTrue(clients.Contains(c2));
        }
        [TestCase]
        public void GetAll_CreateTwo_ReturnedCopyOfCollection()
        {
            Client c1 = Client.CreateNew(ivanovII);
            Client c2 = Client.CreateNew(petrovPP);

            IList<Client> clients_old = new List<Client>(Client.GetAll());
            clients_old.Remove(c1);
            IReadOnlyList<Client> clients = Client.GetAll();

            Assert.AreEqual(2, clients.Count);
            Assert.AreEqual(1, clients_old.Count);
        }

        [TestCase]
        public void Equals_DifferentObjectsWithSameNameAndDifferentPhonNumbers_IsDifferent()
        {
            Client c1 = Client.CreateNew(ivanovII, phoneNumber2);
            Client c2 = Client.CreateNew(ivanovII, phoneNumber3);
            Assert.IsFalse(c1.Equals(c2));
        }
        [TestCase]
        public void Equals_EqualsWithStudentWitSameNameAndPhoneNumber_IsDifferent()
        {
            Client c = Client.CreateNew(ivanovII, phoneNumber2);
            Student s = Student.CreateNew(ivanovII, c, phoneNumber2);
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
            var clients = Client.FilterByName("Иванов Иван Иванович");
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
                Payment.CreateNew(new DateTime(2020, 10, 10), 100000, paymentOrder, "123")
            );
            Assert.AreEqual(100000, client.BalanceInKopeks);
            client.MakePayment(
                Payment.CreateNew(new DateTime(2020, 10, 15), 200000, paymentOrder, "125")
            );
            Assert.AreEqual(300000, client.BalanceInKopeks);
        }
        [TestCase]
        public void MakePayment_ClientMakesPayment_ClientUpdateCorrectly()
        {
            var client = CreateClient();
            var oldUpdCnt = clients.UpdatesCount;
            client.MakePayment(
                Payment.CreateNew(new DateTime(2020, 10, 10), 100000, paymentOrder, "123")
            );
            Assert.AreEqual(oldUpdCnt + 1, clients.UpdatesCount);
            client.MakePayment(
                Payment.CreateNew(new DateTime(2020, 10, 15), 200000, paymentOrder, "125")
            );
            Assert.AreEqual(oldUpdCnt + 2, clients.UpdatesCount);
        }
        [TestCase]
        public void MakePayment_NegativePayment_ThrowsException()
        {
            var client = CreateClient();
            var exception = Assert.Throws<ArgumentException>(
                () => client.MakePayment(
                    Payment.CreateNew(new DateTime(2020, 10, 10), -100000, paymentOrder, "123")
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
                    Payment.CreateNew(new DateTime(2020, 10, 10), 0, paymentOrder, "123")
                )
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "payment should has positive value"
            ));
        }


        [TestCase]
        public void RemovePayment_ClientRemovePayment_BalanceDecrease()
        {
            var client = CreateClient();
            Payment p = Payment.CreateNew(
                new DateTime(2020, 10, 10),
                100000, 
                paymentOrder, 
                "123"
            );
            client.MakePayment(p);
            client.MakePayment(
                Payment.CreateNew(
                    new DateTime(2020, 10, 15),
                    200000, 
                    paymentOrder, 
                    "125"
                )
            );
            client.RemovePayment(p);
            Assert.AreEqual(200000, client.BalanceInKopeks);
        }
        [TestCase]
        public void RemovePayment_ClientRemovePayment_ClientUpdated()
        {
            var client = CreateClient();
            Payment p = Payment.CreateNew(
                new DateTime(2020, 10, 10),
                100000,
                paymentOrder,
                "123"
            );
            var oldUpdCnt = clients.UpdatesCount;
            client.MakePayment(p);
            client.MakePayment(
                Payment.CreateNew(new DateTime(2020, 10, 15), 200000, paymentOrder, "125")
            );
            client.RemovePayment(p);
            Assert.AreEqual(oldUpdCnt + 3, clients.UpdatesCount);
        }
        [TestCase]
        public void RemovePayment_RemoveNullPayment_ThrowsException()
        {
            var client = CreateClient();

            var exception = Assert.Throws<ArgumentException>(
                () => client.RemovePayment(null)
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "payment can't be null"
            ));
        }
        [TestCase]
        public void RemovePayment_RemoveNotexistentPayment_ThrowsException()
        {
            var client = CreateClient();

            var exception = Assert.Throws<ArgumentException>(
                () => client.RemovePayment(
                    Payment.CreateNew(new DateTime(2020, 10, 15), 200000, paymentOrder, "125")
                )
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "payment is not exist"
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
            Assert.AreEqual(ivanovII, client.PersonData.FullName);
        }
        [TestCase]
        public void PhoneNumber_CreateNewClientWithoutPhoneNumber_PhoneNumberIsEmpty()
        {
            var client = CreateClient();
            Assert.AreEqual(null, client.PersonData.PhoneNumber);
        }
        [TestCase]
        public void PhoneNumber_CreateNewClientWithPhoneNumber_PhoneNumberIsCorrect()
        {
            var client = CreateClientWithPhoneNumber();
            Assert.AreEqual(phoneNumber1, client.PersonData.PhoneNumber);
        }
        [TestCase]
        public void PhoneNumber_ChangePhoneNumber_PhoneNumberIsCorrect()
        {
            var client = CreateClient();
            Assert.AreEqual(null, client.PersonData.PhoneNumber);
            client.PersonData.ChangePhoneNumber(phoneNumber1);
            Assert.AreEqual(phoneNumber1, client.PersonData.PhoneNumber);
        }
        [TestCase]
        public void PhoneNumber_ChangePhoneNumber_ClientIsUpdated()
        {
            var client = CreateClient();
            int updOld = clients.UpdatesCount;
            client.PersonData.ChangePhoneNumber(phoneNumber1);
            Assert.AreEqual(updOld + 1, clients.UpdatesCount);
        }

        [TestCase]
        public void PhoneNumber_ChangeFullName_FullNameIsCorrect()
        {
            var client = CreateClient();
            Assert.AreEqual(ivanovII, client.PersonData.FullName);
            client.PersonData.ChangeFullName(petrovPP);
            Assert.AreEqual(petrovPP, client.PersonData.FullName);
        }
        [TestCase]
        public void PhoneNumber_ChangeFullName_ClientIsUpdated()
        {
            var client = CreateClient();
            int updOld = clients.UpdatesCount;
            client.PersonData.ChangeFullName(petrovPP);
            Assert.AreEqual(updOld + 1, clients.UpdatesCount);
        }

        [TestCase]
        public void DecreaseBalance_summIsNegative_throwsException()
        {
            var client = CreateClient();
            var exception = Assert.Throws<ArgumentException>(
                () => client.DecreaseBalance(-100000)
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "value of decreasing balance can't be negative"
                )
            );
        }
        [TestCase]
        public void DecreaseBalance_summIsPositive_balanceDecrease()
        {
            var client = CreateClient();
            client.DecreaseBalance(100000);
            Assert.AreEqual(-100000, client.BalanceInKopeks);
        }
        [TestCase]
        public void DecreaseBalance_summIsPositive_clientUpdated()
        {
            var client = CreateClient();

            var uc = clients.UpdatesCount;
            client.DecreaseBalance(100000);
            Assert.AreEqual(uc + 1, clients.UpdatesCount);
        }

        [TestCase]
        public void IncreaseBalance_summIsNegative_throwsException()
        {
            var client = CreateClient();
            var exception = Assert.Throws<ArgumentException>(
                () => client.IncreaseBalance(-100000)
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "value of increasing balance can't be negative"
                )
            );
        }
        [TestCase]
        public void IncreaseBalance_summIsPositive_balanceIncrease()
        {
            var client = CreateClient();
            client.IncreaseBalance(100000);
            Assert.AreEqual(100000, client.BalanceInKopeks);
        }
        [TestCase]
        public void IncreaseBalance_summIsPositive_clientUpdated()
        {
            var client = CreateClient();

            var uc = clients.UpdatesCount;
            client.IncreaseBalance(100000);
            Assert.AreEqual(uc + 1, clients.UpdatesCount);
        }

        [TestCase]
        public void UpdateNotes_UpdateWithNotNull_UpdatecountIncrease()
        {
            var client = CreateClient();
            int oldUpdCnt = clients.UpdatesCount;
            client.UpdateNote("new note");
            Assert.AreEqual(oldUpdCnt + 1, clients.UpdatesCount);
        }

        private Client CreateClientWithPhoneNumber()
        {
            var c = Client.CreateNew(
                ivanovII, phoneNumber1
            );
            return c;
        }
        private List<Payment> CreatePayments()
        {
            var payments = new List<Payment>();
            payments.Add(
                Payment.CreateNew(
                    new DateTime(2020, 10, 10), 
                    100000, 
                    paymentOrder,
                    "123"
                )
            );
            payments.Add(
                Payment.CreateNew(
                    new DateTime(2020, 10, 15),
                    200000,
                    paymentOrder,
                    "125"
                )
            );
            payments.Add(
                Payment.CreateNew(
                    new DateTime(2020, 10, 21),
                    300000,
                    paymentOrder,
                    "127"
                )
            );
            payments.Add(
                Payment.CreateNew(
                    new DateTime(2021, 1, 12),
                    150000,
                    paymentOrder,
                    "129"
                )
            );
            payments.Add(
                Payment.CreateNew(
                    new DateTime(2021, 2, 15),
                    250000,
                    paymentOrder,
                    "131"
                )
            );
            return payments;
        }
        private Client CreateClient()
        {
            var c = Client.CreateNew(ivanovII);
            return c;
        }
        private List<Client> CreateClients()
        {
            List<Client> clients = new List<Client>();

            clients.Add(Client.CreateNew(ivanovII));
            clients.Add(Client.CreateNew(petrovPP, phoneNumber4));
            clients.Add(Client.CreateNew(petrovaAI));
            clients.Add(Client.CreateNew(petrovPP, phoneNumber5));

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
