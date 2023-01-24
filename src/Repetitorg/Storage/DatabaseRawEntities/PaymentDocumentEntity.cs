using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.DatabaseRawEntities
{
    class PaymentDocumentEntity : EntityWithId
    {
        public string DocumentType { get; set; }
    }
}
