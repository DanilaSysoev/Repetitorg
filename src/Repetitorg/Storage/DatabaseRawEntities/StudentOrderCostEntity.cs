using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.DatabaseRawEntities
{
    class StudentOrderCostEntity
    {
        public long StudentId { get; set; }
        public long OrderId { get; set; }
        public long CostInCopexPerHour { get; set; }
    }
}
