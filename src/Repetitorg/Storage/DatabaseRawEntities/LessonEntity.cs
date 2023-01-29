using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.DatabaseRawEntities
{
    class LessonEntity : DatabaseEntity
    {
        public string DateTime { get; set; }
        public int LengthInMinutes { get; set; }
        public long OrderId { get; set; }
        public long StatusId { get; set; }
        public long MovedOnId { get; set; }
        public long MovedFromId { get; set; }
    }
}
