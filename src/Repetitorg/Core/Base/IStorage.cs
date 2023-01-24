using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core.Base
{
    public interface IStorage<T>
    {
        IReadOnlyList<T> GetAll();
        IList<T> Filter(Predicate<T> predicate);

        long Add(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
