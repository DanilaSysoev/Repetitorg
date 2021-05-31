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
    }
}
