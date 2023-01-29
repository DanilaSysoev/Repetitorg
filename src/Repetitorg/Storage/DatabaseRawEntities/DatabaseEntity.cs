using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.DatabaseRawEntities
{
    abstract class DatabaseEntity
    {
        public long Id { get; set; }
    }
}
