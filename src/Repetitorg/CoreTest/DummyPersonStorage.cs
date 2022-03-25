using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repetitorg.CoreTest
{
    class DummyPersonStorage<T> : IPersonStorage<T> where T : Person
    {
        List<T> entities;

        public DummyPersonStorage()
        {
            entities = new List<T>();
        }

        public void Add(T entity)
        {
            entities.Add(entity);
        }

        public IReadOnlyList<T> GetAll()
        {
            return entities;
        }
    }
}
