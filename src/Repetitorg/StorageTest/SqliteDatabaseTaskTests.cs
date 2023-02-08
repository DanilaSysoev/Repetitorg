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
    class SqliteDatabaseTaskTests
    {
        private Project p1;

        private SqliteDatabase database;
        private static int dbNumber = 0;
        private static string dbName = "testTaskDb";
        [SetUp]
        public void Initialize()
        {
            dbNumber++;
            InitializeDatabase();
            p1 = Project.CreateNew("p1");
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
            CreateOneTask();
            Assert.DoesNotThrow(
                () => InitializeDatabase()
            );
        }
        [TestCase]
        public void CreateProjects_ReconnectAfterCreating_DatabaseContainsAll()
        {
            CreateTwoDifferentTasks();
            var tasksBefore = Task.GetAll();
            InitializeDatabase();
            var tasksAfter = Task.GetAll();
            foreach (var task in tasksBefore)
                Assert.IsTrue(tasksAfter.Contains(task));
            foreach (var task in tasksAfter)
                Assert.IsTrue(tasksBefore.Contains(task));
            Assert.AreEqual(2, tasksAfter.Count);
            Assert.AreEqual(2, tasksBefore.Count);
        }
        [TestCase]
        public void Remove_ReconnectAfterCreatingAndRemoving_DatabaseCorrect()
        {
            CreateTwoDifferentTasks();
            var tasksBefore = Task.GetAll();
            Task.Remove(tasksBefore[1]);
            Assert.AreEqual(1, Task.Count);

            InitializeDatabase();
            var tasksAfter = Task.GetAll();
            Assert.AreEqual(1, tasksAfter.Count);

            Assert.True(tasksBefore[0].Equals(tasksAfter[0]));
        }
        [TestCase]
        public void Update_ReconnectAfterCreatingAndUpdatingComplete_DatabaseCorrect()
        {
            CreateOneTask();
            var tasksBefore = Task.GetAll();
            tasksBefore[0].Complete();
            Assert.IsTrue(Task.GetAll()[0].Completed);
            InitializeDatabase();
            var tasksAfter = Task.GetAll();
            Assert.True(tasksBefore[0].Equals(tasksAfter[0]));
        }
        [TestCase]
        public void Update_ReconnectAfterCreatingAndUpdatingNotes_DatabaseCorrect()
        {
            CreateOneTask();
            var tasksBefore = Task.GetAll();
            var newNote = "test note";
            tasksBefore[0].UpdateNote(newNote);
            Assert.AreEqual(newNote, Task.GetAll()[0].Note);
            InitializeDatabase();
            var tasksAfter = Task.GetAll();
            Assert.True(tasksBefore[0].Note.Equals(tasksAfter[0].Note));
        }
        [TestCase]
        public void Update_ReconnectAfterCreatingAndUpdatingProject_DatabaseCorrect()
        {
            CreateOneTask();
            var tasksBefore = Task.GetAll();
            tasksBefore[0].AttachToProject(p1);
            Assert.AreEqual(p1, Task.GetAll()[0].Project);
            InitializeDatabase();
            var tasksAfter = Task.GetAll();
            Assert.AreEqual(p1, Task.GetAll()[0].Project);
        }
        [TestCase]
        public void Update_ReconnectAfterAttachingAndDropProject_DatabaseCorrect()
        {
            CreateOneTask();
            var tasksBefore = Task.GetAll();
            tasksBefore[0].AttachToProject(p1);            
            tasksBefore[0].AttachToProject(null);
            InitializeDatabase();
            var tasksAfter = Task.GetAll();
            Assert.AreEqual(null, Task.GetAll()[0].Project);
        }

        private static void CreateOneTask()
        {
            Task.CreateNew("t1", new DateTime(2023, 2, 7));
        }
        private static void CreateTwoDifferentTasks()
        {
            CreateOneTask();
            Task.CreateNew("t2", new DateTime(2023, 2, 10));
        }
    }
}
