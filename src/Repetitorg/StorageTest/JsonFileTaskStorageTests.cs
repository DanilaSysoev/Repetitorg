using NUnit.Framework;
using Repetitorg.Core;
using Repetitorg.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.StorageTest
{
    [TestFixture]
    class JsonFileTaskStorageTests
    {
        [TearDown]
        public void Clear()
        {
            Task.Clear();
            var storage = new JsonFileTasksStorage(TEST_DATA_PATH);
            Task.Save(storage);
        }

        [TestCase]
        public void SaveLoad_SaveLoadThreeTasks_TasksCountEqualsThree()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.AddOnDate("2020/12/30 test task 3", new DateTime(2020, 12, 30));

            var storage = new JsonFileTasksStorage(TEST_DATA_PATH);
            Task.Save(storage);
            Task.Clear();
            Task.Load(storage);

            Assert.AreEqual(3, Task.TasksCount);
        }
        [TestCase]
        public void SaveLoad_SaveLoadTreeTasks_TasksContainsAllAfterLoading()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.AddOnDate("2020/12/30 test task 3", new DateTime(2020, 12, 30));

            var storage = new JsonFileTasksStorage(TEST_DATA_PATH);
            Task.Save(storage);
            Task.Clear();
            Task.Load(storage);

            var tasks = Task.GetByDate(new DateTime(2020, 12, 30));
            Assert.AreEqual(3, tasks.Count);
            Assert.IsTrue(tasks.Contains(task1));
            Assert.IsTrue(tasks.Contains(task2));
            Assert.IsTrue(tasks.Contains(task3));
        }
        [TestCase]
        public void SaveLoad_CompleteAndSaveLoad_CompleteStatusSaved()
        {
            Task task1 = Task.AddOnDate("NEW 2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("NEW 2020/11/10 test task 2", new DateTime(2020, 11, 10));
            Task task3 = Task.AddOnDate("NEW 2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Task.Complete(task2);
            var storage = new JsonFileTasksStorage(TEST_DATA_PATH);
            Task.Save(storage);
            Task.Clear();
            Task.Load(storage);

            Assert.IsTrue(Task.GetByDate(new DateTime(2020, 11, 10))[0].Completed);

            Assert.IsFalse(Task.GetByDate(new DateTime(2020, 12, 30))[0].Completed);
            Assert.IsFalse(Task.GetByDate(new DateTime(2020, 10, 20))[0].Completed);
        }

        private const string TEST_DATA_PATH = "D:\\YandexDisk\\YandexDisk\\Danila\\Work\\Repetitorg";
    }
}
