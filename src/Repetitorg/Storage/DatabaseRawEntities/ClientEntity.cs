using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.DatabaseRawEntities
{
    class ClientEntity : NotableDatabaseEntity
    {
        public long BalanceInKopeks { get; set; }
        public long PersonDataId { get; set; }
    }
}
