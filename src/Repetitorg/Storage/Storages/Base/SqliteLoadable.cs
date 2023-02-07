using Microsoft.Data.Sqlite;
using Repetitorg.Core.Base;
using Storage.SQLite.DatabaseRawEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storage.SQLite.Storages.Base
{
    abstract class SqliteLoadable<E> : RawSqliteInterface, ILoadable, IStorage<E>
    {
        protected Dictionary<long, E> entities;

        public SqliteLoadable(SqliteDatabase database)
            : base(database)
        {
            entities = new Dictionary<long, E>();
        }

        public IList<E> Filter(Predicate<E> predicate)
        {
            return FilterByPredicate(entities.Values, predicate);
        }

        public IReadOnlyList<E> GetAll()
        {
            return new List<E>(entities.Values);
        }
        internal E Get(long id)
        {
            return entities[id];
        }

        public abstract void Load();
        public abstract long Add(E entity);
        public abstract void Update(E entity);
        public abstract void Remove(E entity);
    }
}
