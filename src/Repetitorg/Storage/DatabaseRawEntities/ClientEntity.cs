using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.DatabaseRawEntities
{
    class ClientEntity : EntityWithId
    {
        public long BalanceInCopex { get; set; }
        public long PersonDataId { get; set; }
    }
}
