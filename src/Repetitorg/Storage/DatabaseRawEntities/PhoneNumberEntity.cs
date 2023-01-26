using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.DatabaseRawEntities
{
    class PhoneNumberEntity : EntityWithId
    {
        public int CountryCode { get; set; }
        public int OperatorCode { get; set; }
        public long Number { get; set; }

        public override string ToString()
        {
            return string.Format("+{0} ({1}) {2}", CountryCode, OperatorCode, Number);
        }
    }
}
