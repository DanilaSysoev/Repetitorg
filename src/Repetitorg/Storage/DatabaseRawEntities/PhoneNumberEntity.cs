using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.DatabaseRawEntities
{
    class PhoneNumberEntity : DatabaseEntity
    {
        public string CountryCode { get; set; }
        public string OperatorCode { get; set; }
        public string Number { get; set; }

        public override string ToString()
        {
            return string.Format("+{0} ({1}) {2}", CountryCode, OperatorCode, Number);
        }
    }
}
