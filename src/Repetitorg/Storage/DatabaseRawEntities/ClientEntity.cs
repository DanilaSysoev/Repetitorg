using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.DatabaseRawEntities
{
    class ClientEntity : DatabaseEntity
    {
        public long BalanceInKopeks { get; set; }
        public long PersonDataId { get; set; }
        public long? NoteId { get; set; }
    }
}
