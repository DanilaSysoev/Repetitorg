using Microsoft.Data.Sqlite;
using Repetitorg.Core;
using Repetitorg.Core.Base;
using Storage.SQLite.DatabaseRawEntities;
using Storage.SQLite.Storages.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class ProjectSqliteStorage : SqliteLoadable<Project>
    {
        public ProjectSqliteStorage(SqliteDatabase database)
            : base(database, "Project")
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
                tableName,
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
            using (var connection =
                   new SqliteConnection(string.Format("Data Source={0}", database.PathToDb))
               )
            {
                connection.Open();
                var projectEntities = ReadEntities(
                    "Task", connection, BuildProjectEntity
                );

                CreateAndLinkObjects(
                    projectEntities
                );

                connection.Close();
            }
        }
        private void CreateAndLinkObjects(List<ProjectEntity> projectEntities)
        {
            entities = new Dictionary<long, Project>();
            foreach (var projectEntity in projectEntities)
            {
                entities.Add(
                    projectEntity.Id,
                    Project.CreateLoaded(
                        projectEntity.Id,
                        projectEntity.Name,
                        projectEntity.Completed == 1,
                        database.NoteStorage.Get(projectEntity.NoteId)
                    )
                );
            }
        }
        private ProjectEntity BuildProjectEntity(SqliteDataReader reader)
        {
            return new ProjectEntity
            {
                Id = reader.GetInt64(0),
                Name = reader.GetString(1),
                Completed = reader.GetInt32(2),
                NoteId = reader.GetInt64(3)
            };
        }

        public override void Remove(Project entity)
        {
            ProjectEntity projectEntity = 
                ReadEntity(tableName, BuildProjectEntity, entity.Id);

            RemoveEntity(projectEntity.Id, tableName);
            if (projectEntity.NoteId != null)
                RemoveEntity(projectEntity.NoteId.Value, NoteTableName);

            entities.Remove(entity.Id);
        }

        public override void Update(Project entity)
        {
            ProjectEntity oldProject = ReadEntity(tableName, BuildProjectEntity, entity.Id);
            NoteEntity oldNote = database.NoteStorage.GetEntity(oldProject.NoteId);

            UpdateProjectEntity(entity, oldProject);
            UpdateNote(entity, tableName, entity.Note, oldNote);
            entities[entity.Id] = entity;
        }

        private void UpdateProjectEntity(Project project, ProjectEntity oldProject)
        {
            if (oldProject.Completed == (project.Completed ? 1 : 0))
                return;
            UpdateSet(
                oldProject.Id,
                tableName,
                new string[] { "completed" },
                new object[] {
                    project.Completed ? 1 : 0
                }
            );
        }
    }
}
