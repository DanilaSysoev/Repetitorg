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
        protected SqliteDatabase database;
        public SqliteLoadable(SqliteDatabase database)
        {
            this.database = database;
        }

        public abstract void Load();

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
        ) where T : DatabaseEntity
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

        public T ReadEntity<T>(
            string tableName,
            Func<SqliteDataReader, T> entityBuilder,
            long id
        )
        {
            T result;
            using (var connection =
                new SqliteConnection(string.Format("Data Source={0}", database.PathToDb))
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

        public void RemoveEntity(
            long id,
            string tableName
        )
        {
            using (var connection =
                new SqliteConnection(string.Format("Data Source={0}", database.PathToDb))
            )
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    "DELETE FROM " + tableName + " " +
                    "WHERE Id = " + id + ";";
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

        public long InsertInto(
            string tableName,
            string[] columnNames,
            object[] values
        )
        {
            string insertLine = BuildInsertList(columnNames, values);
            long id = -1;
            using (var connection =
                new SqliteConnection(string.Format("Data Source={0}", database.PathToDb))
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

        public void UpdateSet(
            long id,
            string tableName,
            string[] columnNames,
            object[] values
        )
        {
            string updateList = BuildUpdateList(columnNames, values);            
            using (var connection =
                new SqliteConnection(string.Format("Data Source={0}", database.PathToDb))
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

        private static string BuildInsertList(
            string[] columns,
            object[] values
        )
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
        private static string BuildUpdateList(
            string[] columns, 
            object[] values
        )
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

        protected long? InsertNote(string note)
        {
            if (note != "")
                return InsertInto(
                    "Note",
                    new string[] { "noteText" },
                    new object[] { note }
                );
            return null;
        }

        protected void UpdateNote(
            IId entity, string tableName, string note, NoteEntity oldNote
        )
        {
            if (oldNote == null && note == "")
                return;
            if (oldNote == null)
                InsertNewNoteAndUpdateNoteId(entity, tableName, note);
            else if (note == "")
                RemoveEntity(oldNote.Id, "Note");
            else
                UpdateNoteData(note, oldNote);
        }
        private void UpdateNoteData(string note, NoteEntity oldNote)
        {
            if (note.Equals(oldNote.Text))
                return;
            UpdateSet(
                oldNote.Id,
                "Note",
                new string[] { "noteText" },
                new object[] { note }
            );
        }
        private void InsertNewNoteAndUpdateNoteId(
            IId entity,
            string tableName,
            string note
        )
        {
            long noteId = InsertInto(
                "Note",
                new string[] { "noteText" },
                new object[] { note }
            );
            UpdateSet(
                entity.Id,
                tableName,
                new string[] { "noteId" },
                new object[] { noteId }
            );
        }
    }
}
