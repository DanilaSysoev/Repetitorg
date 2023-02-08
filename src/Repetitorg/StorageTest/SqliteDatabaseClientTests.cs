using NUnit.Framework;
using Repetitorg.Core;
using Storage.SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Repetitorg.StorageTest
{
    [TestFixture]
    class SqliteDatabaseClientTests
    {
        private SqliteDatabase database;
        private static int dbNumber = 0;
        private static string dbName = "testClientDb";
        [SetUp]
        public void Initialize()
        {
            dbNumber++;
            InitializeDatabase();
        }

        [OneTimeSetUp]
        public void DestroyDatabased()
        {
            int i = 1;
            var filename = dbName + i + ".sqlite";
            while (File.Exists(filename))
            {
                File.Delete(filename);
                ++i;
                filename = dbName + i + ".sqlite";
            }
        }
        private void InitializeDatabase()
        {
            database = new SqliteDatabase();
            database.Initialize(dbName + dbNumber + ".sqlite");
        }

        [TestCase]
        public void CreateDatabase_CreateNewDatabase_CreateWithoutErrors()
        {
            Assert.IsTrue(true);          
        }
        [TestCase]
        public void AddClient_AddWithNullPhoneNumber_AddWithoutErrors()
        {            
            Assert.DoesNotThrow(
                () => Client.CreateNew(
                    new FullName
                    (
                        firstName: "Danila",
                        lastName: "Sysoev",
                        patronymic: "Yurevich"
                    )
                )
            );
        }
        [TestCase]
        public void InitializeNonEmptyDb_ClientWithoutPhone_InitializeWithoutErrors()
        {
            Client.CreateNew(
                new FullName
                (
                    firstName: "Danila",
                    lastName: "Sysoev",
                    patronymic: "Yurevich"
                )
            );
            Assert.DoesNotThrow(
                () => InitializeDatabase()
            );
        }
        [TestCase]
        public void InitializeNonEmptyDb_ClientWithPhone_InitializeWithoutErrors()
        {
            CreateOneClientWithFullData();
            Assert.DoesNotThrow(
                () => InitializeDatabase()
            );
        }

        [TestCase]
        public void CreateClients_ReconnectAfterCreating_DatabaseContainsAll()
        {
            CreateTwoDifferentClients();
            var clientsBefore = Client.GetAll();
            InitializeDatabase();
            var clientsAfter = Client.GetAll();
            foreach (var client in clientsBefore)
                Assert.IsTrue(clientsAfter.Contains(client));
            foreach (var client in clientsAfter)
                Assert.IsTrue(clientsBefore.Contains(client));
            Assert.AreEqual(2, clientsAfter.Count);
            Assert.AreEqual(2, clientsBefore.Count);
        }
        [TestCase]
        public void Remove_ReconnectAfterCreatingAndRemoving_DatabaseCorrect()
        {
            CreateTwoDifferentClients();
            var clientsBefore = Client.GetAll();
            Client.Remove(clientsBefore[1]);
            Assert.AreEqual(1, Client.Count);

            InitializeDatabase();
            var clientsAfter = Client.GetAll();
            Assert.AreEqual(1, clientsAfter.Count);

            Assert.True(clientsBefore[0].Equals(clientsAfter[0]));
        }
        [TestCase]
        public void Update_ReconnectAfterCreatingAndUpdatingPersonData_DatabaseCorrect()
        {
            CreateOneClientWithFullData();
            var clientsBefore = Client.GetAll();
            var newFullName = new FullName("Ben", "Jonson", "");
            clientsBefore[0].PersonData.ChangeFullName(
                newFullName
            );
            Assert.AreEqual(newFullName, Client.GetAll()[0].PersonData.FullName);
            InitializeDatabase();
            var clientsAfter = Client.GetAll();
            Assert.True(clientsBefore[0].Equals(clientsAfter[0]));
        }
        [TestCase]
        public void Update_ReconnectAfterCreatingAndUpdatingPhoneNumber_DatabaseCorrect()
        {
            CreateOneClientWithFullData();
            var clientsBefore = Client.GetAll();
            var newPhone = new PhoneNumber("7", "900", "1234567");
            clientsBefore[0].PersonData.ChangePhoneNumber(
                newPhone
            );
            Assert.AreEqual(newPhone, Client.GetAll()[0].PersonData.PhoneNumber);
            InitializeDatabase();
            var clientsAfter = Client.GetAll();
            Assert.True(clientsBefore[0].Equals(clientsAfter[0]));
        }
        [TestCase]
        public void Update_ReconnectAfterCreatingAndUpdatingPhoneNumberOnNull_DatabaseCorrect()
        {
            CreateOneClientWithFullData();
            var clientsBefore = Client.GetAll();
            clientsBefore[0].PersonData.ChangePhoneNumber(
                null
            );
            Assert.IsNull(Client.GetAll()[0].PersonData.PhoneNumber);
            InitializeDatabase();
            var clientsAfter = Client.GetAll();
            Assert.True(clientsBefore[0].Equals(clientsAfter[0]));
        }
        [TestCase]
        public void Update_ReconnectAfterCreatingAndUpdatingNotes_DatabaseCorrect()
        {
            CreateOneClientWithFullData();
            var clientsBefore = Client.GetAll();
            var newNote = "test note";
            clientsBefore[0].UpdateNote(newNote);
            Assert.AreEqual(newNote, Client.GetAll()[0].Note);
            InitializeDatabase();
            var clientsAfter = Client.GetAll();
            Assert.True(clientsBefore[0].Note.Equals(clientsAfter[0].Note));
        }


        private static void CreateOneClientWithFullData()
        {
            Client.CreateNew(
                new FullName
                (
                    firstName: "Danila",
                    lastName: "Sysoev",
                    patronymic: "Yurevich"
                ),
                new PhoneNumber
                (
                    countryCode: "7",
                    operatorCode: "910",
                    number: "3400743"
                )
            );
        }
        private static void CreateTwoDifferentClients()
        {
            CreateOneClientWithFullData();
            Client.CreateNew(
                new FullName
                (
                    firstName: "Alesya",
                    lastName: "Sysoeva",
                    patronymic: "Vladinirovna"
                ),
                new PhoneNumber
                (
                    countryCode: "7",
                    operatorCode: "950",
                    number: "7521550"
                )
            );
        }
    }
}
