using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.DatabaseRawEntities
{
    class StudentEntity : DatabaseEntity
    {
        public long PersonDataId { get; set; }
        public long ClientId { get; set; }
    }
}
