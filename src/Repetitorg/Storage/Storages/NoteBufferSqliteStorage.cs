using Microsoft.Data.Sqlite;
using Storage.SQLite.DatabaseRawEntities;
using Storage.SQLite.Storages.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class NoteBufferSqliteStorage : RawSqliteInterface, ILoadable
    {
        protected Dictionary<long, NoteEntity> entities;
        public NoteBufferSqliteStorage(SqliteDatabase database)
            : base(database)
        {
            this.database = database;
        }

        public string Get(long? id)
        {
            if (id == null)
                return "";
            return entities[id.Value].Text;
        }
        public NoteEntity GetEntity(long? id)
        {
            if (id == null)
                return null;
            return entities[id.Value];
        }
        public void UpdateNote(long id, string text)
        {
            entities[id].Text = text;
        }
        public void RemoveNote(long id)
        {
            entities.Remove(id);
        }
        public void AddNote(NoteEntity note)
        {
            entities.Add(note.Id, note);
        }

        public void Load()
        {
            using (var connection =
                   new SqliteConnection(string.Format("Data Source={0}", database.PathToDb))
               )
            {
                connection.Open();
                entities = ReadEntitiesToDict(
                    "Note", connection, BuildNoteEntity
                );

                connection.Close();
            }
        }

        private NoteEntity BuildNoteEntity(SqliteDataReader reader)
        {
            var note = new NoteEntity
            {
                Id = reader.GetInt64(0),
                Text = reader.GetString(1)
            };
            return note;
        }
    }
}
