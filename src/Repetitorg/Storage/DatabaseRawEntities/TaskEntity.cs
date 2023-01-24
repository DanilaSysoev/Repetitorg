using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.DatabaseRawEntities
{
    class TaskEntity : EntityWithId
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public int Completed { get; set; }
        public long ProjectId { get; set; }
    }
}
