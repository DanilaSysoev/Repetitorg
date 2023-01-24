using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.DatabaseRawEntities
{
    class LessonStatusEntity : EntityWithId
    {
        public string Status { get; set; }
    }
}
