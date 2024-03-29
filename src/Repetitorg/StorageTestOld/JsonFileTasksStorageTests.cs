﻿using NUnit.Framework;
using Repetitorg.Core;
using Repetitorg.Storage;
using System;

namespace Repetitorg.StorageTest
{
    [TestFixture]
    class JsonFileTasksStorageTests
    {
        [TearDown]
        public void Clear()
        {
            Task.Clear();
            var storage = new JsonFileStorage<Task>(TEST_DATA_PATH);
            Task.Save(storage);
        }

        [TestCase]
        public void Load_LoadUnexistingTasks_ReturnEmptyCollection()
        {
            var storage = new JsonFileStorage<Task>(TEST_DATA_PATH);
            Task.Load(storage);

            Assert.AreEqual(0, Task.TasksCount);
        }
        [TestCase]
        public void SaveLoad_SaveLoadTreeTasks_TasksContainsAllAfterLoading()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.AddOnDate("2020/12/30 test task 3", new DateTime(2020, 12, 30));

            var storage = new JsonFileStorage<Task>(TEST_DATA_PATH);
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
            var storage = new JsonFileStorage<Task>(TEST_DATA_PATH);
            Task.Save(storage);
            Task.Clear();
            Task.Load(storage);

            Assert.IsTrue(Task.GetByDate(new DateTime(2020, 11, 10))[0].Completed);

            Assert.IsFalse(Task.GetByDate(new DateTime(2020, 12, 30))[0].Completed);
            Assert.IsFalse(Task.GetByDate(new DateTime(2020, 10, 20))[0].Completed);
        }
        [TestCase]
        public void SaveLoad_AttachToTaskAndSaveLoad_AttachSaved()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("2020/11/10 test task 2", new DateTime(2020, 11, 10));
            Task task3 = Task.AddOnDate("2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Project p1 = Project.Add("Test project 1");
            Project p2 = Project.Add("Test project 2");

            Task.AttachToProject(task1, p1);
            Task.AttachToProject(task2, p2);
            var storage = new JsonFileStorage<Task>(TEST_DATA_PATH);
            Task.Save(storage);
            Task.Clear();
            Task.Load(storage);

            Assert.AreEqual(p1, Task.GetByDate(new DateTime(2020, 12, 30))[0].Project);
            Assert.AreEqual(p2, Task.GetByDate(new DateTime(2020, 11, 10))[0].Project);
            Assert.IsNull(Task.GetByDate(new DateTime(2020, 10, 20))[0].Project);
        }

        private const string TEST_DATA_PATH = "D:\\YandexDisk\\YandexDisk\\Danila\\Work\\Repetitorg";
    }
}
