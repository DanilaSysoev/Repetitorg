using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core.Base
{
    public interface IDatabase
    {
        IStorage<T> Entities<T>();
    }
}
