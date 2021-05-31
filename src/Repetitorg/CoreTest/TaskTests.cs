using Core;
using Core.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreTest
{
    [TestFixture]
    class TaskTests
    {
        [TearDown]
        public void Clear()
        {
            Task.Clear();
            Task.Save(TEST_DATA_PATH);
        }

        [TestCase]
        public void AddOnDate_SimpleAddTask_TasksCountIncrease()
        {
            Assert.AreEqual(0, Task.TasksCount);
            Task task = Task.AddOnDate("2020/12/30 test task", new DateTime(2020, 12, 30));
            Assert.AreEqual(1, Task.TasksCount);
        }
        [TestCase]
        public void AddOnDate_SimpleAddTask_TasksAddedOnCorrectDate()
        {
            Task task = Task.AddOnDate("2020/12/30 test task", new DateTime(2020, 12, 30));
            List<Task> tasks = Task.GetByDate(new DateTime(2020, 12, 30));
            Assert.AreEqual(task, tasks[0]);
            Assert.AreEqual(1, Task.TasksCount);
        }
        [TestCase]
        public void AddOnDate_AddThreeTask_AllTasksExist()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.AddOnDate("2020/12/30 test task 3", new DateTime(2020, 12, 30));
            List<Task> tasks = Task.GetByDate(new DateTime(2020, 12, 30));

            Assert.AreEqual(3, tasks.Count);
            Assert.IsTrue(tasks.Contains(task1));
            Assert.IsTrue(tasks.Contains(task2));
            Assert.IsTrue(tasks.Contains(task3));
        }
        [TestCase]
        public void AddOnDate_AddFourTaskOnDifferentDate_AllTasksExist()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("2020/12/30 test task 2", new DateTime(2019, 11, 20));
            Task task3 = Task.AddOnDate("2020/12/30 test task 3", new DateTime(2018, 10, 10));
            Task task4 = Task.AddOnDate("2020/12/30 test task 3", new DateTime(2017, 10, 5));
            List<Task> tasks1 = Task.GetByDate(new DateTime(2020, 12, 30));
            List<Task> tasks2 = Task.GetByDate(new DateTime(2019, 11, 20));
            List<Task> tasks3 = Task.GetByDate(new DateTime(2018, 10, 10));
            List<Task> tasks4 = Task.GetByDate(new DateTime(2017, 10, 5));

            Assert.AreEqual(4, Task.TasksCount);
            Assert.AreEqual(1, tasks1.Count);
            Assert.AreEqual(1, tasks2.Count);
            Assert.AreEqual(1, tasks3.Count);
            Assert.AreEqual(1, tasks4.Count);
            Assert.IsTrue(tasks1.Contains(task1));
            Assert.IsTrue(tasks2.Contains(task2));
            Assert.IsTrue(tasks3.Contains(task3));
            Assert.IsTrue(tasks4.Contains(task4));
        }

        [TestCase]
        public void SaveLoad_SaveLoadTreeTasks_TasksCountEqualsThree()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.AddOnDate("2020/12/30 test task 3", new DateTime(2020, 12, 30));
            Task.Save(TEST_DATA_PATH);
            Task.Clear();
            Task.Load(TEST_DATA_PATH);

            Assert.AreEqual(3, Task.TasksCount);
        }
        [TestCase]
        public void SaveLoad_SaveLoadTreeTasks_TasksContainsAllAfterLoading()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.AddOnDate("2020/12/30 test task 3", new DateTime(2020, 12, 30));
            Task.Save(TEST_DATA_PATH);
            Task.Clear();
            Task.Load(TEST_DATA_PATH);

            var tasks = Task.GetByDate(new DateTime(2020, 12, 30));
            Assert.AreEqual(3, tasks.Count);
            Assert.IsTrue(tasks.Contains(task1));
            Assert.IsTrue(tasks.Contains(task2));
            Assert.IsTrue(tasks.Contains(task3));
        }
        [TestCase]
        public void GetAll_AddThreeOnSameDate_ReturnAll()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.AddOnDate("2020/12/30 test task 3", new DateTime(2020, 12, 30));

            var tasks = Task.GetAll();
            Assert.AreEqual(3, tasks.Count);
            Assert.IsTrue(tasks.Contains(task1));
            Assert.IsTrue(tasks.Contains(task2));
            Assert.IsTrue(tasks.Contains(task3));
        }
        [TestCase]
        public void GetAll_AddThreeOnDifferentDate_ReturnAll()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("2020/11/10 test task 2", new DateTime(2020, 11, 10));
            Task task3 = Task.AddOnDate("2020/10/20 test task 3", new DateTime(2020, 10, 20));

            var tasks = Task.GetAll();
            Assert.AreEqual(3, tasks.Count);
            Assert.IsTrue(tasks.Contains(task1));
            Assert.IsTrue(tasks.Contains(task2));
            Assert.IsTrue(tasks.Contains(task3));
        }
        [TestCase]
        public void GetAll_AddThreeOnDifferentDate_ReturncopyOfCollection()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("2020/11/10 test task 2", new DateTime(2020, 11, 10));
            Task task3 = Task.AddOnDate("2020/10/20 test task 3", new DateTime(2020, 10, 20));

            var tasks = Task.GetAll();
            tasks.Remove(task1);
            var tasksOld = Task.GetAll();

            Assert.AreEqual(2, tasks.Count);
            Assert.AreEqual(3, tasksOld.Count);
        }
        [TestCase]
        public void GetByDate_GettingAllBydate_ReturnCopyOfCollection()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.AddOnDate("2020/12/30 test task 3", new DateTime(2020, 12, 30));

            var tasks = Task.GetByDate(new DateTime(2020, 12, 30));
            tasks.Remove(task2);
            var tasksOld = Task.GetByDate(new DateTime(2020, 12, 30));

            Assert.AreEqual(2, tasks.Count);
            Assert.AreEqual(3, tasksOld.Count);
        }


        [TestCase]
        public void Clear_ClearAfterAdding_TasksCountEqualsZero()
        {
            Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task.AddOnDate("2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task.AddOnDate("2020/12/30 test task 3", new DateTime(2020, 12, 30));

            Assert.AreEqual(3, Task.TasksCount);
            Task.Clear();
            Assert.AreEqual(0, Task.TasksCount);
        }
        [TestCase]
        public void Clear_ClearAfterAdding_TasksIsempty()
        {
            Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task.AddOnDate("2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task.AddOnDate("2020/12/30 test task 3", new DateTime(2020, 12, 30));

            Assert.AreEqual(3, Task.TasksCount);
            Task.Clear();
            Assert.AreEqual(0, Task.GetAll().Count);
        }
        [TestCase]
        public void AddOnDate_Duplicate_ThrowsException()
        {
            Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            var exception = Assert.Throws<TaskAlreadyExistException>(() => Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30)));
            Assert.IsTrue(
                exception.Message.Contains(
                    string.Format("The task with name \"2020/12/30 test task 1\" has already been defined for date \"{0}\"", new DateTime(2020, 12, 30))
                )
            );
        }
        [TestCase]
        public void Remove_RemoveExisting_CountDecrease()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("2020/11/10 test task 2", new DateTime(2020, 11, 10));
            Task task3 = Task.AddOnDate("2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Task.Remove(task2);
            Assert.AreEqual(2, Task.TasksCount);
        }
        [TestCase]
        public void Remove_RemoveExisting_CountDecreaseInGetByDate()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.AddOnDate("2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Task.Remove(task2);

            Assert.AreEqual(1, Task.GetByDate(new DateTime(2020, 12, 30)).Count);
        }
        [TestCase]
        public void Remove_RemoveExisting_CountDecreaseInGetAll()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.AddOnDate("2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Task.Remove(task2);

            Assert.AreEqual(2, Task.GetAll().Count);
        }
        [TestCase]
        public void Remove_RemoveNonexistent_NothingHappens()
        {
            Task taskOld = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task.Clear();

            Task task1 = Task.AddOnDate("NEW 2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("NEW 2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.AddOnDate("NEW 2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Assert.AreEqual(3, Task.TasksCount);
            Task.Remove(taskOld);
            Assert.AreEqual(3, Task.TasksCount);
        }

        [TestCase]
        public void Complete_CompleteExisting_CompleteSuccess()
        {
            Task task1 = Task.AddOnDate("NEW 2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("NEW 2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.AddOnDate("NEW 2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Assert.IsFalse(task2.Completed);
            Task.Complete(task2);
            Assert.IsTrue(task2.Completed);
        }
        [TestCase]
        public void Complete_CompleteAndSaveLoad_CompleteStatusSaved()
        {
            Task task1 = Task.AddOnDate("NEW 2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("NEW 2020/11/10 test task 2", new DateTime(2020, 11, 10));
            Task task3 = Task.AddOnDate("NEW 2020/10/20 test task 3", new DateTime(2020, 10, 20));
                        
            Task.Complete(task2);
            Task.Save(TEST_DATA_PATH);
            Task.Clear();
            Task.Load(TEST_DATA_PATH);

            Assert.IsTrue(Task.GetByDate(new DateTime(2020, 11, 10))[0].Completed);

            Assert.IsFalse(Task.GetByDate(new DateTime(2020, 12, 30))[0].Completed);
            Assert.IsFalse(Task.GetByDate(new DateTime(2020, 10, 20))[0].Completed);
        }

        [TestCase]
        public void Setup_SetupAfterClearing_SetupSuccess()
        {
            Task task1 = Task.AddOnDate("NEW 2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("NEW 2020/11/10 test task 2", new DateTime(2020, 11, 10));
            Task task3 = Task.AddOnDate("NEW 2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Task.Complete(task2);
            Task.Clear();
            Task.Setup(new List<Task> { task1, task2, task3 });

            Assert.AreEqual(3, Task.TasksCount);
            Assert.AreEqual(3, Task.GetAll().Count);
            Assert.IsTrue(Task.GetAll().Contains(task1));
            Assert.IsTrue(Task.GetAll().Contains(task2));
            Assert.IsTrue(Task.GetAll().Contains(task3));

            Assert.IsTrue(Task.GetByDate(new DateTime(2020, 11, 10))[0].Completed);

            Assert.IsFalse(Task.GetByDate(new DateTime(2020, 12, 30))[0].Completed);
            Assert.IsFalse(Task.GetByDate(new DateTime(2020, 10, 20))[0].Completed);
        }
        [TestCase]
        public void Setup_SetupWithoutClearing_ThrowingException()
        {
            Task task1 = Task.AddOnDate("NEW 2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("NEW 2020/11/10 test task 2", new DateTime(2020, 11, 10));
            Task task3 = Task.AddOnDate("NEW 2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Task.Complete(task2);

            var exception = Assert.Throws<InvalidOperationException>(() => Task.Setup(new List<Task> { task1, task2, task3 }));
            Assert.IsTrue(exception.Message.Contains("Setup can be calld only for clear Task collection"));
        }

        private const string TEST_DATA_PATH = "D:\\YandexDisk\\YandexDisk\\Danila\\Work\\Repetitorg";
    }
}
