using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.DatabaseRawEntities
{
    class PaymentEntity : EntityWithId
    {
        public string Date { get; set; }
        public long SummInCopex { get; set; }
        public long DocumentTypeId { get; set; }
        public string DocumentId { get; set; }
        public long ClientId { get; set; }

    }
}
