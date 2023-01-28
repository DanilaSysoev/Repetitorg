using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.DatabaseRawEntities
{
    class PersonDataEntity : EntityWithId
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public long? PhoneNumberId { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", LastName, FirstName, Patronymic);
        }
    }
}
