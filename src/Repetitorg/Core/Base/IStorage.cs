using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core.Base
{
    public interface IStorage<T>
    {
        IReadOnlyList<T> GetAll();
    }
}
