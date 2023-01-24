using NUnit.Framework;
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
            if(File.Exists("test.db"))
                File.Delete("test.db");

            Assert.False(File.Exists("test.db"));
            SqliteDatabase database = new SqliteDatabase();            
            Assert.DoesNotThrow(
                () => database.Initialize("test.db")
            );            
        }
    }
}
