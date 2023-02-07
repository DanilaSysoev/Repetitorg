using Repetitorg.Core;
using Repetitorg.Core.Base;
using Storage.SQLite.Storages.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class ProjectSqliteStorage : SqliteLoadable<Project>
    {
        public ProjectSqliteStorage(SqliteDatabase database)
            : base(database)
        {
        }

        public override long Add(Project entity)
        {
            long? noteId = InsertNote(entity.Note);
            long taskId = InsertProject(entity, noteId);
            entities.Add(taskId, entity);
            return taskId;
        }
        private long InsertProject(Project entity, long? noteId)
        {
            return InsertInto(
                "Project",
                new string[] {
                    "name",
                    "completed",
                    "noteId"
                },
                new object[]
                {
                    entity.Name,
                    entity.Completed ? 1 : 0,
                    noteId
                }
            );
        }

        public override void Load()
        {
        }

        public override void Remove(Project entity)
        {
            throw new NotImplementedException();
        }

        public override void Update(Project entity)
        {
            throw new NotImplementedException();
        }
    }
}
