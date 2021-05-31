using NUnit.Framework;
using Repetitorg.Core;
using Repetitorg.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repetitorg.StorageTest
{
    [TestFixture]
    class JsonFileProjectsStorageTests
    {
        [TearDown]
        public void Clear()
        {
            Project.Clear();
            var storage = new JsonFileStorage<Project>(TEST_DATA_PATH);
            Project.Save(storage);
        }

        [TestCase]
        public void SaveLoad_AddThreeSaveThenClearLoad_AllThreeExist()
        {
            Project p1 = Project.Add("Test Project 1");
            Project p2 = Project.Add("Test Project 2");
            Project p3 = Project.Add("Test Project 3");

            var storage = new JsonFileStorage<Project>(TEST_DATA_PATH);
            Project.Save(storage);
            Project.Clear();
            Project.Load(storage);

            Assert.AreEqual(3, Project.ProjectsCount);
            Assert.IsTrue(Project.GetAll().Contains(p1));
            Assert.IsTrue(Project.GetAll().Contains(p2));
            Assert.IsTrue(Project.GetAll().Contains(p3));
        }
        [TestCase]
        public void SaveLoad_CompleteAndSaveLoad_CompleteStatusSaved()
        {
            Project p1 = Project.Add("Test project 1");
            Project p2 = Project.Add("Test project 2");
            Project p3 = Project.Add("Test project 3");

            Project.Complete(p1);
            var storage = new JsonFileStorage<Project>(TEST_DATA_PATH);
            Project.Save(storage);
            Project.Clear();
            Project.Load(storage);

            Assert.IsTrue(Project.FindByName("Test project 1")[0].Completed);

            Assert.IsFalse(Project.FindByName("Test project 2")[0].Completed);
            Assert.IsFalse(Project.FindByName("Test project 3")[0].Completed);
        }

        private const string TEST_DATA_PATH = "D:\\YandexDisk\\YandexDisk\\Danila\\Work\\Repetitorg";
    }
}
