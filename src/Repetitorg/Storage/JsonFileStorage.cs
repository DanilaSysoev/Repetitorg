using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Storage
{
    public class JsonFileStorage<T> : IStorage<T>
    {
        public void Save(List<T> projects)
        {
            throw new NotImplementedException();
        }

        public List<T> Load()
        {
            throw new NotImplementedException();
        }
    }
}
