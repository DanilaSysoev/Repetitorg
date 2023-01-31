using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    public class PhoneNumber
    {
        public string CountryCode { get; private set; }
        public string OperatorCode { get; private set; }
        public string Number { get; private set; }

        public PhoneNumber(
            string countryCode,
            string operatorCode,
            string number
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
            return CountryCode.Equals(other.CountryCode) &&
                   OperatorCode.Equals(other.OperatorCode) &&
                   Number.Equals(other.Number);
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(CountryCode, OperatorCode, Number);
        }
    }
}
