using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core.Base
{
    public interface IStorage<T>
    {
        IReadOnlyList<T> GetAll();
        IReadOnlyList<T> Filter(Predicate<T> predicate);

        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
