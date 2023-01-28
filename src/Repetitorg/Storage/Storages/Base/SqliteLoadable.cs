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
        )where T : EntityWithId
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

        public static T ReadEntity<T>(
            string tableName,
            string pathToDb,
            Func<SqliteDataReader, T> entityBuilder,
            long id
        )
        {
            T result;
            using (var connection =
                new SqliteConnection(string.Format("Data Source={0}", pathToDb))
            )
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    "SELECT * FROM " + tableName + " WHERE id=" + id + ";";
                var reader = command.ExecuteReader();
                var entities = new List<T>();
                if (!reader.Read())
                    result = default(T);
                else 
                    result = entityBuilder(reader);
                connection.Close();
            }
            return result;
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
            string insertLine = BuildInsertList(columnNames, values);
            long id = -1;
            using (var connection =
                new SqliteConnection(string.Format("Data Source={0}", pathToDb))
            )
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    "INSERT INTO " + tableName + " " + insertLine + ";";
                if (command.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("Entity from " + tableName + " not removed");

                command.CommandText = "SELECT last_insert_rowid()";
                long lastRowId = (long)command.ExecuteScalar();

                command.CommandText = "SELECT id FROM " + tableName + " WHERE ROWID = " + lastRowId;
                id = (long)command.ExecuteScalar();
            }
            return id;
        }

        public static void UpdateSet(
            string tableName,
            string[] columnNames,
            object[] values,
            string pathToDb
        )
        {
            string updateList = BuildUpdateList(columnNames, values);            
            using (var connection =
                new SqliteConnection(string.Format("Data Source={0}", pathToDb))
            )
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    "UPDATE " + tableName + " SET " + updateList + ";";
                if (command.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("Entity from " + tableName + " not removed");
            }            
        }

        private static string BuildInsertList(string[] columns, object[] values)
        {
            StringBuilder resultString = new StringBuilder("(");
            for (int i = 0; i < values.Length; ++i)
            {
                if(values[i] != null)
                    resultString.Append(columns[i]).Append(", ");
            }
            resultString.Remove(resultString.Length - 2, 2);
            resultString.Append(") VALUES (");

            for (int i = 0; i < values.Length; ++i)
            {
                if (values[i] != null)
                    resultString.Append("'").Append(values[i]).Append("', ");
            }
            resultString.Remove(resultString.Length - 2, 2);
            resultString.Append(")");
            return resultString.ToString();
        }
        private static string BuildUpdateList(string[] columns, object[] values)
        {
            StringBuilder resultString = new StringBuilder();
            for (int i = 0; i < values.Length; ++i)
            {
                resultString.Append(columns[i])
                            .Append(" = '")
                            .Append(values[i])
                            .Append("', ");
            }
            resultString.Remove(resultString.Length - 2, 2);
            return resultString.ToString();
        }
    }
}
