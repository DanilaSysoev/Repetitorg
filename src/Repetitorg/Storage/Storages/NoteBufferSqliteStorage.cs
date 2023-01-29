﻿using Microsoft.Data.Sqlite;
using Storage.SQLite.DatabaseRawEntities;
using Storage.SQLite.Storages.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class NoteBufferSqliteStorage : SqliteLoadable
    {
        private Dictionary<long, NoteEntity> notes;
        public NoteEntity GetNote(long id)
        {
            return notes[id];
        }
        public void UpdateNote(long id, string text)
        {
            notes[id].Text = text;
        }
        public void RemoveNote(long id)
        {
            notes.Remove(id);
        }
        public void AddNote(NoteEntity note)
        {
            notes.Add(note.Id, note);
        }


        public override void Load(string pathToDb)
        {
            using (var connection =
                   new SqliteConnection(string.Format("Data Source={0}", pathToDb))
               )
            {
                connection.Open();
                var noteEntities = ReadEntitiesToDict(
                    "PhoneNumber", connection, BuildNoteEntity
                );

                connection.Close();
            }
        }

        private EntityWithId BuildNoteEntity(SqliteDataReader reader)
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
