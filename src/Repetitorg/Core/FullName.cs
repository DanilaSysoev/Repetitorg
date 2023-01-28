using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    public class FullName
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Patronymic { get; private set; }

        public FullName(
            string firstName,
            string lastName,
            string patronymic
        )
        {
            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", LastName, FirstName, Patronymic);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FullName) || obj == null)
                return false;
            var other = obj as FullName;
            return FirstName.Equals(other.FirstName) &&
                    LastName.Equals(other.LastName) &&
                    Patronymic.Equals(other.Patronymic);
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(FirstName, LastName, Patronymic);
        }
    }
}
