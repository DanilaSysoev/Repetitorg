using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core.Base
{
    public interface IStorage<T>
    {
        void Save(List<T> projects);
        List<T> Load();
    }
}
