using System;
using System.Collections.Generic;
using System.Text;

namespace Storage
{
    class EntityWrite<T>
    {
        public long Id { get; private set; }
        public T Entity { get; private set; }

        internal EntityWrite(long id, T entity)
        {
            Id = id;
            Entity = entity;
        }
    }
}
