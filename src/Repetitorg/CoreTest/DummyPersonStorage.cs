using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repetitorg.CoreTest
{
    class DummyPersonStorage<T> : IStorage<T>
    {
        public int UpdatesCount { get; private set; }
        public int AddCount { get; private set; }
        List<T> entities;

        public DummyPersonStorage()
        {
            entities = new List<T>();
            UpdatesCount = 0;
            AddCount = 0;
        }

        public void Add(T entity)
        {
            entities.Add(entity);
            AddCount += 1;
        }

        public IReadOnlyList<T> GetAll()
        {
            return entities;
        }

        public void Update(T person)
        {
            UpdatesCount += 1;
        }

        public void Remove(T entity)
        {
            entities.Remove(entity);
        }

        public IList<T> Filter(Predicate<T> predicate)
        {
            return (from person in entities
                    where predicate(person)
                    select person).ToList();
        }
    }
}
