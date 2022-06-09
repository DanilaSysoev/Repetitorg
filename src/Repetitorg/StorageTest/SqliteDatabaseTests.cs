using NUnit.Framework;
using Storage.SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.StorageTest
{
    [TestFixture]
    class SqliteDatabaseTests
    {
        [TestCase]
        public void testT()
        {
            var db = new SqliteDatabase();
            db.Initialize("testdb.sqlite");
        }
    }
}
