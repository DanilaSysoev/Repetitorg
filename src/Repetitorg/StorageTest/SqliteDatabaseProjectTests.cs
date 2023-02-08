using NUnit.Framework;
using Repetitorg.Core;
using Storage.SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Repetitorg.StorageTest
{
    [TestFixture]
    class SqliteDatabaseProjectTests
    {
        private SqliteDatabase database;
        private static int dbNumber = 0;
        private static string dbName = "testProjectDb";
        [SetUp]
        public void Initialize()
        {
            dbNumber++;
            InitializeDatabase();
        }

        [OneTimeSetUp]
        public void DestroyDatabased()
        {
            int i = 1;
            var filename = dbName + i + ".sqlite";
            while (File.Exists(filename))
            {
                File.Delete(filename);
                ++i;
                filename = dbName + i + ".sqlite";
            }
        }
        private void InitializeDatabase()
        {
            database = new SqliteDatabase();
            database.Initialize(dbName + dbNumber + ".sqlite");
        }

        [TestCase]
        public void InitializeNonEmptyDb_ProjectWithName_InitializeWithoutErrors()
        {
            CreateOneProject();
            Assert.DoesNotThrow(
                () => InitializeDatabase()
            );
        }
        [TestCase]
        public void CreateProjects_ReconnectAfterCreating_DatabaseContainsAll()
        {
            CreateTwoDifferentProjects();
            var projectsBefore = Project.GetAll();
            InitializeDatabase();
            var projectsAfter = Project.GetAll();
            foreach (var project in projectsBefore)
                Assert.IsTrue(projectsAfter.Contains(project));
            foreach (var project in projectsAfter)
                Assert.IsTrue(projectsBefore.Contains(project));
            Assert.AreEqual(2, projectsAfter.Count);
            Assert.AreEqual(2, projectsBefore.Count);
        }
        [TestCase]
        public void Remove_ReconnectAfterCreatingAndRemoving_DatabaseCorrect()
        {
            CreateTwoDifferentProjects();
            var projectsBefore = Project.GetAll();
            Project.Remove(projectsBefore[1]);
            Assert.AreEqual(1, Project.Count);

            InitializeDatabase();
            var projectsAfter = Project.GetAll();
            Assert.AreEqual(1, projectsAfter.Count);

            Assert.True(projectsBefore[0].Equals(projectsAfter[0]));
        }
        [TestCase]
        public void Update_ReconnectAfterCreatingAndUpdatingComplete_DatabaseCorrect()
        {
            CreateOneProject();
            var projectsBefore = Project.GetAll();
            projectsBefore[0].Complete();
            Assert.IsTrue(Project.GetAll()[0].Completed);
            InitializeDatabase();
            var projectsAfter = Project.GetAll();
            Assert.True(projectsBefore[0].Equals(projectsAfter[0]));
        }        
        [TestCase]
        public void Update_ReconnectAfterCreatingAndUpdatingNotes_DatabaseCorrect()
        {
            CreateOneProject();
            var projectsBefore = Project.GetAll();
            var newNote = "test note";
            projectsBefore[0].UpdateNote(newNote);
            Assert.AreEqual(newNote, Project.GetAll()[0].Note);
            InitializeDatabase();
            var projectsAfter = Project.GetAll();
            Assert.True(projectsBefore[0].Note.Equals(projectsAfter[0].Note));
        }


        private static void CreateOneProject()
        {
            Project.CreateNew(
                "p1"
            );
        }
        private static void CreateTwoDifferentProjects()
        {
            CreateOneProject();
            Project.CreateNew(
                "p2"
            );
        }
    }
}
