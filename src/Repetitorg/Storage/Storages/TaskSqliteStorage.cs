using Microsoft.Data.Sqlite;
using Repetitorg.Core;
using Repetitorg.Core.Base;
using Storage.SQLite.DatabaseRawEntities;
using Storage.SQLite.Storages.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Storage.SQLite.Storages
{
    class TaskSqliteStorage : SqliteLoadable<Task>
    {
        public TaskSqliteStorage(SqliteDatabase database)
            : base(database, "Task")
        {
        }

        public override long Add(Task entity)
        {
            long? noteId = InsertNote(entity.Note);
            long? projectId = null;
            if (entity.Project != null)
                projectId = entity.Project.Id;
            long taskId = InsertTask(entity, noteId, projectId);
            entities.Add(taskId, entity);
            return taskId;
        }
        private long InsertTask(Task entity, long? noteId, long? projectId)
        {
            return InsertInto(
                tableName,
                new string[] {
                    "name",
                    "date",
                    "completed",
                    "projectId",
                    "noteId"
                },
                new object[]
                {
                    entity.Name,
                    entity.Date.ToString("yyyyMMdd"),
                    entity.Completed ? 1 : 0,
                    projectId,
                    noteId
                }
            );
        }

        public override void Load()
        {
            using (var connection =
                new SqliteConnection(string.Format("Data Source={0}", database.PathToDb))
            )
            {
                connection.Open();
                var taskEntities = ReadEntities(
                    tableName, connection, BuildTaskEntity
                );

                CreateAndLinkObjects(
                    taskEntities
                );

                connection.Close();
            }
        }

        private void CreateAndLinkObjects(List<TaskEntity> taskEntities)
        {
            entities = new Dictionary<long, Task>();
            foreach(var taskEntity in taskEntities)
            {
                Project project = null;
                if (taskEntity.ProjectId != null)
                    project = database.ProjectStorage.Get(taskEntity.ProjectId.Value);
                entities.Add(
                    taskEntity.Id,
                    Task.CreateLoaded(
                        taskEntity.Id,
                        taskEntity.Name,
                        DateTime.ParseExact(taskEntity.Date, "yyyyMMdd", CultureInfo.InvariantCulture),
                        taskEntity.Completed == 1,
                        project,
                        database.NoteStorage.Get(taskEntity.NoteId)
                    )
                );
            }
        }
        private TaskEntity BuildTaskEntity(SqliteDataReader reader)
        {
            long? noteId = null;
            if (!reader.IsDBNull(5))
                noteId = reader.GetInt64(5);
            long? projectId = null;
            if (!reader.IsDBNull(4))
                projectId = reader.GetInt64(4);
            return new TaskEntity
            {
                Id = reader.GetInt64(0),
                Name = reader.GetString(1),
                Date = reader.GetString(2),
                Completed = reader.GetInt32(3),
                ProjectId = projectId,
                NoteId = noteId
            };
        }

        public override void Remove(Task entity)
        {
            TaskEntity taskEntity = 
                ReadEntity(tableName, BuildTaskEntity, entity.Id);

            RemoveEntity(taskEntity.Id, tableName);
            if (taskEntity.NoteId != null)
                RemoveEntity(taskEntity.NoteId.Value, NoteTableName);

            entities.Remove(entity.Id);
        }

        public override void Update(Task entity)
        {
            TaskEntity oldTask = ReadEntity(tableName, BuildTaskEntity, entity.Id);
            NoteEntity oldNote = database.NoteStorage.GetEntity(oldTask.NoteId);

            UpdateTaskEntity(entity, oldTask);
            UpdateNote(entity, tableName, entity.Note, oldNote);
            entities[entity.Id] = entity;
        }
        private void UpdateTaskEntity(Task task, TaskEntity oldTask)
        {
            if (oldTask.Completed == (task.Completed ? 1 : 0) &&
                (oldTask.ProjectId == null && task.Project == null ||
                task.Project != null && oldTask.ProjectId == task.Project.Id))
                return;

            long? projectId = null;
            if (task.Project != null)
                projectId = task.Project.Id;
            UpdateSet(
                oldTask.Id,
                tableName,
                new string[] { "completed", "projectId" },
                new object[] {
                    task.Completed ? 1 : 0,
                    projectId
                }
            );
        }
    }
}
