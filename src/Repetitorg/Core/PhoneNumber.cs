using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    public class PhoneNumber
    {
        public long CountryCode { get; private set; }
        public long OperatorCode { get; private set; }
        public long Number { get; private set; }

        public PhoneNumber(
            long countryCode,
            long operatorCode,
            long number
        )
        {
            CountryCode = countryCode;
            OperatorCode = operatorCode;
            Number = number;
        }

        public override string ToString()
        {
            return string.Format("+{0} ({1}) {2}", CountryCode, OperatorCode, Number);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PhoneNumber) || obj == null)
                return false;
            var other = obj as PhoneNumber;
            return CountryCode == other.CountryCode &&
                   OperatorCode == other.OperatorCode &&
                   Number == other.Number;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(CountryCode, OperatorCode, Number);
        }
    }
}
