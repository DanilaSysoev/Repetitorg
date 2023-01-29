using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.DatabaseRawEntities
{
    class ProjectEntity : DatabaseEntity
    {
        public string Name { get; set; }
        public int Completed { get; set; }
    }
}
