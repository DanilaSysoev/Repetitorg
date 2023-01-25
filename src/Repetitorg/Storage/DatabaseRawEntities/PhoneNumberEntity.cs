using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.DatabaseRawEntities
{
    class PhoneNumberEntity
    {
        public long Id { get; set; }
        public int CountryCode { get; set; }
        public int OperatorCode { get; set; }
        public long Number { get; set; }
    }
}
