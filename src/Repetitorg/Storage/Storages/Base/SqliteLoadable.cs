using Microsoft.Data.Sqlite;
using Repetitorg.Core.Base;
using Storage.SQLite.DatabaseRawEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storage.SQLite.Storages.Base
{
    abstract class SqliteLoadable : ILoadable
    {
        public abstract void Load(string pathToDb);

        public static List<T> ReadEntities<T>(
            string tableName,
            SqliteConnection connection,
            Func<SqliteDataReader, T> entityBuilder
        )
        {
            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT * FROM " + tableName + ";";
            var reader = command.ExecuteReader();
            var entities = new List<T>();
            while (reader.Read())
            {
                entities.Add(entityBuilder(reader));
            }
            return entities;
        }

        public static Dictionary<long, T> ReadEntitiesToDict<T>(
            string tableName,
            SqliteConnection connection,
            Func<SqliteDataReader, T> entityBuilder
        ) where T : EntityWithId
        {
            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT * FROM " + tableName + ";";
            var reader = command.ExecuteReader();
            var entities = new Dictionary<long, T>();
            while (reader.Read())
            {
                var entity = entityBuilder(reader);
                entities.Add(entity.Id, entity);
            }
            return entities;
        }

        public static void RemoveEntity<T>(
            T entity,
            string tableName,
            string pathToDb
        ) where T : IId
        {
            using (var connection =
                new SqliteConnection(string.Format("Data Source={0}", pathToDb))
            )
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    "DELETE FROM " + tableName + " " +
                    "WHERE Id = " + entity.Id + ";";
                if (command.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("Entity from " + tableName + " not removed");
            }
        }

        public static IList<T> FilterByPredicate<T> (
            IEnumerable<T> collection,
            Predicate<T> predicate
        )
        {
            return (from entity in collection
                    where predicate(entity)
                    select entity).ToList();
        }

        public static long InsertInto(
            string tableName,
            string[] columnNames,
            object[] values,
            string pathToDb
        )
        {
            string columnsString = BuildArgumentList(columnNames);
            string valuesString = BuildArgumentList(values);
            long id = -1;
            using (var connection =
                new SqliteConnection(string.Format("Data Source={0}", pathToDb))
            )
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    "INSERT INTO " + tableName + " " + columnsString + " " +
                    "VALUES " + valuesString + ";";
                if (command.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("Entity from " + tableName + " not removed");

                command.CommandText = "SELECT last_insert_rowid()";
                long lastRowId = (long)command.ExecuteScalar();

                command.CommandText = "SELECT id FROM " + tableName + " WHERE ROWID = " + lastRowId;
                id = (long)command.ExecuteScalar();
            }
            return id;
        }

        private static string BuildArgumentList(object[] values)
        {
            StringBuilder resultString = new StringBuilder("(");
            for (int i = 0; i < values.Length - 1; ++i)
                resultString.Append(values[i]).Append(", ");
            resultString.Append(values[values.Length - 1]).Append(')');
            return resultString.ToString();
        }
    }
}
