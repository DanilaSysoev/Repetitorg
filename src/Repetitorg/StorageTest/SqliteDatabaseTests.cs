using NUnit.Framework;
using Repetitorg.Core;
using Storage.SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Repetitorg.StorageTest
{
    [TestFixture]
    class SqliteDatabaseTests
    {
        [TestCase]
        public void CreateDatabase_CreateNewDatabase_CreateWithoutErrors()
        {
            if(File.Exists("test1.db"))
                File.Delete("test1.db");

            Assert.False(File.Exists("test1.db"));
            SqliteDatabase database = new SqliteDatabase();            
            Assert.DoesNotThrow(
                () => database.Initialize("test1.db")
            );            
        }
        [TestCase]
        public void AddClient_AddWithNullPhoneNumber_AddWithoutErrors()
        {
            if (File.Exists("test2.db"))
                File.Delete("test2.db");

            Assert.False(File.Exists("test2.db"));
            SqliteDatabase database = new SqliteDatabase();
            database.Initialize("test2.db");
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
            if (File.Exists("test3.db"))
                File.Delete("test3.db");

            Assert.False(File.Exists("test3.db"));
            SqliteDatabase database = new SqliteDatabase();
            database.Initialize("test3.db");
            Client.CreateNew(
                new FullName
                (
                    firstName: "Danila",
                    lastName: "Sysoev",
                    patronymic: "Yurevich"
                )
            );
            database = new SqliteDatabase();
            database.Initialize("test3.db");
        }
        [TestCase]
        public void InitializeNonEmptyDb_ClientWithPhone_InitializeWithoutErrors()
        {
            if (File.Exists("test4.db"))
                File.Delete("test4.db");

            Assert.False(File.Exists("test4.db"));
            SqliteDatabase database = new SqliteDatabase();
            database.Initialize("test4.db");
            Client.CreateNew(
                new FullName
                (
                    firstName: "Danila",
                    lastName: "Sysoev",
                    patronymic: "Yurevich"
                ),
                new PhoneNumber
                (
                    countryCode: 7,
                    operatorCode: 910,
                    number: 3400743
                )
            );
            database = new SqliteDatabase();
            database.Initialize("test4.db");
        }
    }
}
