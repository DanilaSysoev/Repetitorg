using Core;
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
            Task.Save("");
            Task.Clear();
            Task.Load("");

            Assert.AreEqual(3, Task.TasksCount);
        }
        [TestCase]
        public void SaveLoad_SaveLoadTreeTasks_TasksContainsAllAfterLoading()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.AddOnDate("2020/12/30 test task 3", new DateTime(2020, 12, 30));
            Task.Save("");
            Task.Clear();
            Task.Load("");

            var tasks = Task.GetByDate(new DateTime(2020, 12, 30));
            Assert.AreEqual(3, tasks.Count);
            Assert.IsTrue(tasks.Contains(task1));
            Assert.IsTrue(tasks.Contains(task2));
            Assert.IsTrue(tasks.Contains(task3));
        }
    }
}
