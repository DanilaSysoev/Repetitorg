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
        public void AddTomorrow_SimpleAddTask_TasksCountIncrease()
        {
            Assert.AreEqual(0, Task.TasksCount);
            Task.AddTomorrow("Tomorrow test task");
            Assert.AreEqual(1, Task.TasksCount);
        }
        [TestCase]
        public void AddTomorrow_SimpleAddTask_TaskAddedWithTomorrowDate()
        {
            Task task = Task.AddTomorrow("Tomorrow test task");
            List<Task> tomorrowTasks = Task.GetTomorrowTasks();
            Assert.AreEqual(task, tomorrowTasks[0]);
        }
        [TestCase]
        public void AddTomorrow_AddThreeTasks_AllTasksExist()
        {
            Task task1 = Task.AddTomorrow("Tomorrow test task 1");
            Task task2 = Task.AddTomorrow("Tomorrow test task 2");
            Task task3 = Task.AddTomorrow("Tomorrow test task 3");
            List<Task> tomorrowTasks = Task.GetTomorrowTasks();

            Assert.AreEqual(3, tomorrowTasks.Count);
            Assert.IsTrue(tomorrowTasks.Contains(task1));
            Assert.IsTrue(tomorrowTasks.Contains(task1));
            Assert.IsTrue(tomorrowTasks.Contains(task1));
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
    }
}
