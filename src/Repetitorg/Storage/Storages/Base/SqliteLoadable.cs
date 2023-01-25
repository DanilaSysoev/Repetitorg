using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
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
    }
}
