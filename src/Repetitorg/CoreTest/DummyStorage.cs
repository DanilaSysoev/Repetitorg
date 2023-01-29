using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.CoreTest
{
    class DummyStorage<T> : IStorage<T>
    {
        private List<T> entities;
        public int UpdatesCount { get; private set; }
        public int AddCount { get; private set; }

        public DummyStorage()
        {
            entities = new List<T>();
            UpdatesCount = 0;
            AddCount = 0;
        }

        public long Add(T task)
        {
            entities.Add(task);
            AddCount += 1;
            return entities.Count;
        }

        public IReadOnlyList<T> GetAll()
        {
            return entities;
        }

        public void Remove(T task)
        {
            entities.Remove(task);
        }

        public void Update(T task)
        {
            UpdatesCount += 1;
        }

        public IList<T> Filter(Predicate<T> predicate)
        {
            return (from task in entities
                    where predicate(task)
                    select task).ToList();
        }
    }
}
