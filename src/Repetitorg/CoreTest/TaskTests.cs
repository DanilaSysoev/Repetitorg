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
            Task.AddTomorrowTask("Tomorrow test task");
            Assert.AreEqual(1, Task.TasksCount);
        }
        [TestCase]
        public void AddTomorrow_SimpleAddTask_TaskAddedWithTomorrowDate()
        {
            Task task = Task.AddTomorrowTask("Tomorrow test task");
            List<Task> tomorrowTasks = Task.GetTomorrowTasks();
            Assert.AreEqual(task, tomorrowTasks[0]);
        }
        [TestCase]
        public void AddTomorrow_AddThreeTasks_AllTasksExist()
        {
            Task task1 = Task.AddTomorrowTask("Tomorrow test task 1");
            Task task2 = Task.AddTomorrowTask("Tomorrow test task 2");
            Task task3 = Task.AddTomorrowTask("Tomorrow test task 3");
            List<Task> tomorrowTasks = Task.GetTomorrowTasks();

            Assert.AreEqual(3, tomorrowTasks.Count);
            Assert.IsTrue(tomorrowTasks.Contains(task1));
            Assert.IsTrue(tomorrowTasks.Contains(task1));
            Assert.IsTrue(tomorrowTasks.Contains(task1));
        }
    }
}
