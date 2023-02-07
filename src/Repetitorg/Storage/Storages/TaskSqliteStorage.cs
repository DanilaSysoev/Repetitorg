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
    class TaskSqliteStorage : SqliteLoadable, IStorage<Task>
    {
        private Dictionary<long, Task> tasks;

        public TaskSqliteStorage(SqliteDatabase database)
            : base(database)
        {
            tasks = new Dictionary<long, Task>();
        }

        public long Add(Task entity)
        {
            long? noteId = InsertNote(entity.Note);
            long taskId = InsertTask(entity, noteId);
            tasks.Add(taskId, entity);
            return taskId;
        }
        private long InsertTask(Task entity, long? noteId)
        {
            return InsertInto(
                "Task",
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
                    entity.Project.Id,
                    noteId
                }
            );
        }

        public IList<Task> Filter(Predicate<Task> predicate)
        {
            return FilterByPredicate(tasks.Values, predicate);
        }

        public IReadOnlyList<Task> GetAll()
        {
            return new List<Task>(tasks.Values);
        }

        public override void Load()
        {
            using (var connection =
                new SqliteConnection(string.Format("Data Source={0}", database.PathToDb))
            )
            {
                connection.Open();
                var taskEntities = ReadEntities(
                    "Task", connection, BuildTaskEntity
                );

                CreateAndLinkObjects(
                    taskEntities
                );

                connection.Close();
            }
        }

        private void CreateAndLinkObjects(List<TaskEntity> taskEntities)
        {
            tasks = new Dictionary<long, Task>();
            foreach(var taskEntity in taskEntities)
            {
                tasks.Add(
                    taskEntity.Id,
                    Task.CreateLoaded(
                        taskEntity.Id,
                        taskEntity.Name,
                        DateTime.ParseExact(taskEntity.Date, "yyyyMMdd", CultureInfo.InvariantCulture),
                        taskEntity.Completed == 1,
                        database.ProjectStorage.Get(taskEntity.ProjectId),
                        database.NoteStorage.Get(taskEntity.NoteId)
                    )
                );
            }
        }

        private TaskEntity BuildTaskEntity(SqliteDataReader reader)
        {
            return new TaskEntity
            {
                Id = reader.GetInt64(0),
                Name = reader.GetString(1),
                Date = reader.GetString(2),
                Completed = reader.GetInt32(3),
                ProjectId = reader.GetInt64(4),
                NoteId = reader.GetInt64(5)
            };
        }

        public void Remove(Task entity)
        {
            TaskEntity taskEntity = ReadEntity("task", BuildTaskEntity, entity.Id);

            RemoveEntity(taskEntity.Id, "Task");
            if (taskEntity.NoteId != null)
                RemoveEntity(taskEntity.NoteId.Value, "Note");

            tasks.Remove(entity.Id);
        }

        public void Update(Task entity)
        {
            TaskEntity oldTask = ReadEntity("Task", BuildTaskEntity, entity.Id);
            NoteEntity oldNote = database.NoteStorage.GetEntity(oldTask.NoteId);

            UpdateTaskEntity(entity, oldTask);
            UpdateNote(entity, "Task", entity.Note, oldNote);
            tasks[entity.Id] = entity;
        }

        private void UpdateTaskEntity(Task task, TaskEntity oldTask)
        {
            if (oldTask.Completed == (task.Completed ? 1 : 0))
                return;
            UpdateSet(
                oldTask.Id,
                "Task",
                new string[] { "co" },
                new object[] {
                    task.Completed ? 1 : 0
                }
            );
        }
    }
}
