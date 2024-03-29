﻿using Repetitorg.Core;
using Repetitorg.Core.Exceptions;
using Repetitorg.Storage;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.CoreTest
{
    [TestFixture]
    class TaskTests
    {
        [TearDown]
        public void Clear()
        {
            Task.Clear();
            Project.Clear();
        }


        [TestCase]
        public void AddOnDate_AddWithNullName_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => Task.AddOnDate(null, new DateTime(2020, 12, 30))
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains("can't add task with null name")
            );
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
        public void GetByDate_DateWithoutTasks_ReturnEmptyCollection()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.AddOnDate("2020/12/30 test task 3", new DateTime(2020, 12, 30));

            var tasks = Task.GetByDate(new DateTime(2019, 12, 30));

            Assert.AreEqual(0, tasks.Count);
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
            var exception = Assert.Throws<InvalidOperationException>(() => Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30)));
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    string.Format("the task with name \"2020/12/30 test task 1\" has already been defined for date \"{0}\"", new DateTime(2020, 12, 30))
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
        public void Remove_RemoveNull_ThrowsException()
        {
            Task task1 = Task.AddOnDate("NEW 2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("NEW 2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.AddOnDate("NEW 2020/10/20 test task 3", new DateTime(2020, 10, 20));

            var exception = Assert.Throws<ArgumentException>(
                () => Task.Remove(null)
            );
            Assert.IsTrue(exception.Message.ToLower().Contains(
                "task can't be null"
            ));
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
            Assert.IsFalse(task1.Completed);
            Assert.IsFalse(task3.Completed);
        }
        [TestCase]
        public void Complete_CompleteAfterRemoveAndRestore_CompleteSuccess()
        {
            Task task1 = Task.AddOnDate("NEW 2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("NEW 2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.AddOnDate("NEW 2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Task.Remove(task1);
            Task task1new = Task.AddOnDate("NEW 2020/12/30 test task 1", new DateTime(2020, 12, 30));

            Task.Complete(task1);
            Assert.IsTrue(task1.Completed);
            Assert.IsTrue(task1new.Completed);
            Assert.IsFalse(task2.Completed);
            Assert.IsFalse(task3.Completed);
        }

        [TestCase]
        public void AttachToProject_AttachNullTask_ThrowsException()
        {
            Project p1 = Project.Add("Test project 1");

            var exception = Assert.Throws<ArgumentException>(
                () => Task.AttachToProject(null, p1)
            );
            Assert.IsTrue(exception.Message.ToLower().Contains(
                "task can't be null"
            ));
        }
        [TestCase]
        public void AttachToProject_AttachNotAttachedTask_AttachSuccess()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));

            Project p1 = Project.Add("Test project 1");

            Assert.IsNull(task1.Project);
            Task.AttachToProject(task1, p1);
            Assert.AreEqual(p1, task1.Project);
        }
        [TestCase]
        public void AttachToProject_AttachAlreadyAttachedTask_TrowsException()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));

            Project p1 = Project.Add("test project 1");
            Project p2 = Project.Add("Test project 2");

            Task.AttachToProject(task1, p1);
            var exception = 
                Assert.Throws<InvalidOperationException>(() => Task.AttachToProject(task1, p2));
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    string.Format("task \"{0}\" already attached to \"{1}\" project", task1, p1)
                )
            );
        }
        [TestCase]
        public void AttachToProject_AttachToSameProject_NothingHappens()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));

            Project p1 = Project.Add("Test project 1");

            Task.AttachToProject(task1, p1);
            Task.AttachToProject(task1, p1);
            Assert.AreEqual(p1, task1.Project);
        }
        [TestCase]
        public void AttachToProject_AttachToNullProjectForAttachedTask_UnattachTask()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));

            Project p1 = Project.Add("Test project 1");

            Task.AttachToProject(task1, p1);
            Task.AttachToProject(task1, null);
            Assert.IsNull(task1.Project);
        }
        [TestCase]
        public void AttachToProject_AttachToNullProjectForNotAttachedTask_NothingHappens()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));

            Project p1 = Project.Add("Test project 1");

            Task.AttachToProject(task1, null);
            Assert.IsNull(task1.Project);
        }
        [TestCase]
        public void AttachToProject_AttachToCompleteProject_ThrowsException()
        {
            Project p = Project.Add("Test project");
            Project.Complete(p);

            Task t = Task.AddOnDate("Test task", new DateTime(2020, 10, 10));

            var exception = Assert.Throws<InvalidOperationException>(
                () => Task.AttachToProject(t, p)
            );
            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can't attach new task to complete project"
            ));
        }


        [TestCase]
        public void GetByProject_GettingByProjectWithotTasks_ReturnEmpty()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("2020/11/30 test task 2", new DateTime(2020, 11, 30));
            Task task3 = Task.AddOnDate("2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Project p1 = Project.Add("Test project 1");
            Project p2 = Project.Add("Test project 2");

            Task.AttachToProject(task1, p2);
            Task.AttachToProject(task3, p2);

            List<Task> tasks = Task.GetByProject(p1);
            Assert.AreEqual(0, tasks.Count);
        }
        [TestCase]
        public void GetByProject_GettingByProjectWithTasks_ReturnAll()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("2020/11/30 test task 2", new DateTime(2020, 11, 30));
            Task task3 = Task.AddOnDate("2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Project p1 = Project.Add("Test project 1");
            Project p2 = Project.Add("Test project 2");

            Task.AttachToProject(task1, p2);
            Task.AttachToProject(task3, p2);

            List<Task> tasks = Task.GetByProject(p2);
            Assert.AreEqual(2, tasks.Count);
            Assert.IsTrue(tasks.Contains(task1));
            Assert.IsTrue(tasks.Contains(task3));
        }
        [TestCase]
        public void GetByProject_GettingByNullProject_ReturnAllWithotProject()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("2020/11/30 test task 2", new DateTime(2020, 11, 30));
            Task task3 = Task.AddOnDate("2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Project p1 = Project.Add("Test project 1");
            Project p2 = Project.Add("Test project 2");

            Task.AttachToProject(task1, p2);

            List<Task> tasks = Task.GetByProject(null);
            Assert.AreEqual(2, tasks.Count);
            Assert.IsTrue(tasks.Contains(task2));
            Assert.IsTrue(tasks.Contains(task3));
        }

        [TestCase]
        public void GetWithotProject_GettingExistingWithoutProject_ReturnAllWithotProject()
        {
            Task task1 = Task.AddOnDate("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.AddOnDate("2020/11/30 test task 2", new DateTime(2020, 11, 30));
            Task task3 = Task.AddOnDate("2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Project p1 = Project.Add("Test project 1");
            Project p2 = Project.Add("Test project 2");

            Task.AttachToProject(task1, p2);

            List<Task> tasks = Task.GetWithoutProject();
            Assert.AreEqual(2, tasks.Count);
            Assert.IsTrue(tasks.Contains(task2));
            Assert.IsTrue(tasks.Contains(task3));
        }
        private const string TEST_DATA_PATH = "D:\\YandexDisk\\YandexDisk\\Danila\\Work\\Repetitorg";
    }
}
