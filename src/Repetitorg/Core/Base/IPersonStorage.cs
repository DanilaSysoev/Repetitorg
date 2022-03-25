using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core.Base
{
    public interface IPersonStorage<T> where T : Person
    {
        IReadOnlyList<T> GetAll();
        void Add(T person);
    }
}
