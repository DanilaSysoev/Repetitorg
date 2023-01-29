using Repetitorg.Core;
using Repetitorg.Core.Exceptions;
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
        private DummyStorage<Task> tasks;
        private DummyStorage<Project> projects;

        [SetUp]
        public void Setup()
        {
            tasks = new DummyStorage<Task>();
            projects = new DummyStorage<Project>();
            Task.SetupStorage(tasks);
            Project.SetupStorage(projects);
        }


        [TestCase]
        public void AddOnDate_AddWithNullName_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => Task.CreateNew(null, new DateTime(2020, 12, 30))
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains("can't add task with null name")
            );
        }
        [TestCase]
        public void AddOnDate_SimpleAddTask_TasksCountIncrease()
        {
            Assert.AreEqual(0, Task.Count);
            Task task = Task.CreateNew("2020/12/30 test task", new DateTime(2020, 12, 30));
            Assert.AreEqual(1, Task.Count);
        }
        [TestCase]
        public void AddOnDate_SimpleAddTask_TaskAddedToStorage()
        {
            Assert.AreEqual(0, Task.Count);
            int pastAC = tasks.AddCount;
            Task task = Task.CreateNew("2020/12/30 test task", new DateTime(2020, 12, 30));
            Assert.AreEqual(pastAC + 1, tasks.AddCount);
        }
        [TestCase]
        public void AddOnDate_SimpleAddTask_TasksAddedOnCorrectDate()
        {
            Task task = Task.CreateNew("2020/12/30 test task", new DateTime(2020, 12, 30));
            IList<Task> tasks = Task.GetByDate(new DateTime(2020, 12, 30));
            Assert.AreEqual(task, tasks[0]);
            Assert.AreEqual(1, Task.Count);
        }
        [TestCase]
        public void AddOnDate_AddThreeTask_AllTasksExist()
        {
            Task task1 = Task.CreateNew("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.CreateNew("2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.CreateNew("2020/12/30 test task 3", new DateTime(2020, 12, 30));
            IList<Task> tasks = Task.GetByDate(new DateTime(2020, 12, 30));

            Assert.AreEqual(3, tasks.Count);
            Assert.IsTrue(tasks.Contains(task1));
            Assert.IsTrue(tasks.Contains(task2));
            Assert.IsTrue(tasks.Contains(task3));
        }
        [TestCase]
        public void AddOnDate_AddFourTaskOnDifferentDate_AllTasksExist()
        {
            Task task1 = Task.CreateNew("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.CreateNew("2020/12/30 test task 2", new DateTime(2019, 11, 20));
            Task task3 = Task.CreateNew("2020/12/30 test task 3", new DateTime(2018, 10, 10));
            Task task4 = Task.CreateNew("2020/12/30 test task 3", new DateTime(2017, 10, 5));
            IList<Task> tasks1 = Task.GetByDate(new DateTime(2020, 12, 30));
            IList<Task> tasks2 = Task.GetByDate(new DateTime(2019, 11, 20));
            IList<Task> tasks3 = Task.GetByDate(new DateTime(2018, 10, 10));
            IList<Task> tasks4 = Task.GetByDate(new DateTime(2017, 10, 5));

            Assert.AreEqual(4, Task.Count);
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
            Task task1 = Task.CreateNew("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.CreateNew("2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.CreateNew("2020/12/30 test task 3", new DateTime(2020, 12, 30));

            var tasks = Task.GetAll();
            Assert.AreEqual(3, tasks.Count);
            Assert.IsTrue(tasks.Contains(task1));
            Assert.IsTrue(tasks.Contains(task2));
            Assert.IsTrue(tasks.Contains(task3));
        }
        [TestCase]
        public void GetAll_AddThreeOnDifferentDate_ReturnAll()
        {
            Task task1 = Task.CreateNew("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.CreateNew("2020/11/10 test task 2", new DateTime(2020, 11, 10));
            Task task3 = Task.CreateNew("2020/10/20 test task 3", new DateTime(2020, 10, 20));

            var tasks = Task.GetAll();
            Assert.AreEqual(3, tasks.Count);
            Assert.IsTrue(tasks.Contains(task1));
            Assert.IsTrue(tasks.Contains(task2));
            Assert.IsTrue(tasks.Contains(task3));
        }

        [TestCase]
        public void GetByDate_DateWithoutTasks_ReturnEmptyCollection()
        {
            Task task1 = Task.CreateNew("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.CreateNew("2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.CreateNew("2020/12/30 test task 3", new DateTime(2020, 12, 30));

            var tasks = Task.GetByDate(new DateTime(2019, 12, 30));

            Assert.AreEqual(0, tasks.Count);
        }


        [TestCase]
        public void AddOnDate_Duplicate_ThrowsException()
        {
            Task.CreateNew("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            var exception = Assert.Throws<InvalidOperationException>(() => Task.CreateNew("2020/12/30 test task 1", new DateTime(2020, 12, 30)));
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    string.Format("the task with name \"2020/12/30 test task 1\" has already been defined for date \"{0}\"", new DateTime(2020, 12, 30))
                )
            );
        }
        [TestCase]
        public void Remove_RemoveExisting_CountDecrease()
        {
            Task task1 = Task.CreateNew("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.CreateNew("2020/11/10 test task 2", new DateTime(2020, 11, 10));
            Task task3 = Task.CreateNew("2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Task.Remove(task2);
            Assert.AreEqual(2, Task.Count);
        }
        [TestCase]
        public void Remove_RemoveExisting_CountDecreaseInGetByDate()
        {
            Task task1 = Task.CreateNew("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.CreateNew("2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.CreateNew("2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Task.Remove(task2);

            Assert.AreEqual(1, Task.GetByDate(new DateTime(2020, 12, 30)).Count);
        }
        [TestCase]
        public void Remove_RemoveExisting_CountDecreaseInGetAll()
        {
            Task task1 = Task.CreateNew("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.CreateNew("2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.CreateNew("2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Task.Remove(task2);

            Assert.AreEqual(2, Task.GetAll().Count);
        }
        [TestCase]
        public void Remove_RemoveNonexistent_NothingHappens()
        {
            Task taskOld = Task.CreateNew("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            tasks = new DummyStorage<Task>();
            Task.SetupStorage(tasks);

            Task task1 = Task.CreateNew("NEW 2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.CreateNew("NEW 2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.CreateNew("NEW 2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Assert.AreEqual(3, Task.Count);
            Task.Remove(taskOld);
            Assert.AreEqual(3, Task.Count);
        }
        [TestCase]
        public void Remove_RemoveNull_ThrowsException()
        {
            Task task1 = Task.CreateNew("NEW 2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.CreateNew("NEW 2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.CreateNew("NEW 2020/10/20 test task 3", new DateTime(2020, 10, 20));

            var exception = Assert.Throws<ArgumentException>(
                () => Task.Remove(null)
            );
            Assert.IsTrue(exception.Message.ToLower().Contains(
                "entity can't be null"
            ));
        }

        [TestCase]
        public void Complete_CompleteExisting_CompleteSuccess()
        {
            Task task1 = Task.CreateNew("NEW 2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.CreateNew("NEW 2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.CreateNew("NEW 2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Assert.IsFalse(task2.Completed);
            task2.Complete();
            Assert.IsTrue(task2.Completed);
            Assert.IsFalse(task1.Completed);
            Assert.IsFalse(task3.Completed);
        }
        [TestCase]
        public void Complete_CompleteExisting_CompletingUpdateTaskInStorage()
        {
            Task task1 = Task.CreateNew("NEW 2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.CreateNew("NEW 2020/12/30 test task 2", new DateTime(2020, 12, 30));
            Task task3 = Task.CreateNew("NEW 2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Assert.AreEqual(0, tasks.UpdatesCount);
            task2.Complete();
            Assert.AreEqual(1, tasks.UpdatesCount);
        }

        [TestCase]
        public void AttachToProject_AttachNotAttachedTask_AttachSuccess()
        {
            Task task1 = Task.CreateNew("2020/12/30 test task 1", new DateTime(2020, 12, 30));

            Project p1 = Project.CreateNew("Test project 1");

            Assert.IsNull(task1.Project);
            task1.AttachToProject(p1);
            Assert.AreEqual(p1, task1.Project);
        }
        [TestCase]
        public void AttachToProject_AttachAlreadyAttachedTask_TrowsException()
        {
            Task task1 = Task.CreateNew("2020/12/30 test task 1", new DateTime(2020, 12, 30));

            Project p1 = Project.CreateNew("test project 1");
            Project p2 = Project.CreateNew("Test project 2");

            task1.AttachToProject(p1);
            var exception = 
                Assert.Throws<InvalidOperationException>(() => task1.AttachToProject(p2));
            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    string.Format("task \"{0}\" already attached to \"{1}\" project", task1, p1)
                )
            );
        }
        [TestCase]
        public void AttachToProject_AttachToSameProject_NothingHappens()
        {
            Task task1 = Task.CreateNew("2020/12/30 test task 1", new DateTime(2020, 12, 30));

            Project p1 = Project.CreateNew("Test project 1");

            task1.AttachToProject(p1);
            task1.AttachToProject(p1);
            Assert.AreEqual(p1, task1.Project);
        }
        [TestCase]
        public void AttachToProject_AttachToNullProjectForAttachedTask_UnattachTask()
        {
            Task task1 = Task.CreateNew("2020/12/30 test task 1", new DateTime(2020, 12, 30));

            Project p1 = Project.CreateNew("Test project 1");

            task1.AttachToProject(p1);
            task1.AttachToProject(null);
            Assert.IsNull(task1.Project);
        }
        [TestCase]
        public void AttachToProject_AttachToNullProjectForNotAttachedTask_NothingHappens()
        {
            Task task1 = Task.CreateNew("2020/12/30 test task 1", new DateTime(2020, 12, 30));

            Project p1 = Project.CreateNew("Test project 1");

            task1.AttachToProject(null);
            Assert.IsNull(task1.Project);
        }
        [TestCase]
        public void AttachToProject_AttachToCompleteProject_ThrowsException()
        {
            Project p = Project.CreateNew("Test project");
            p.Complete();

            Task t = Task.CreateNew("Test task", new DateTime(2020, 10, 10));

            var exception = Assert.Throws<InvalidOperationException>(
                () => t.AttachToProject(p)
            );
            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can't attach new task to complete project"
            ));
        }


        [TestCase]
        public void GetByProject_GettingByProjectWithotTasks_ReturnEmpty()
        {
            Task task1 = Task.CreateNew("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.CreateNew("2020/11/30 test task 2", new DateTime(2020, 11, 30));
            Task task3 = Task.CreateNew("2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Project p1 = Project.CreateNew("Test project 1");
            Project p2 = Project.CreateNew("Test project 2");

            task1.AttachToProject(p2);
            task3.AttachToProject(p2);

            IList<Task> tasks = Task.GetByProject(p1);
            Assert.AreEqual(0, tasks.Count);
        }
        [TestCase]
        public void GetByProject_GettingByProjectWithTasks_ReturnAll()
        {
            Task task1 = Task.CreateNew("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.CreateNew("2020/11/30 test task 2", new DateTime(2020, 11, 30));
            Task task3 = Task.CreateNew("2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Project p1 = Project.CreateNew("Test project 1");
            Project p2 = Project.CreateNew("Test project 2");

            task1.AttachToProject(p2);
            task3.AttachToProject(p2);

            IList<Task> tasks = Task.GetByProject(p2);
            Assert.AreEqual(2, tasks.Count);
            Assert.IsTrue(tasks.Contains(task1));
            Assert.IsTrue(tasks.Contains(task3));
        }
        [TestCase]
        public void GetByProject_GettingByNullProject_ReturnAllWithotProject()
        {
            Task task1 = Task.CreateNew("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.CreateNew("2020/11/30 test task 2", new DateTime(2020, 11, 30));
            Task task3 = Task.CreateNew("2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Project p1 = Project.CreateNew("Test project 1");
            Project p2 = Project.CreateNew("Test project 2");

            task1.AttachToProject(p2);

            IList<Task> tasks = Task.GetByProject(null);
            Assert.AreEqual(2, tasks.Count);
            Assert.IsTrue(tasks.Contains(task2));
            Assert.IsTrue(tasks.Contains(task3));
        }

        [TestCase]
        public void GetWithotProject_GettingExistingWithoutProject_ReturnAllWithotProject()
        {
            Task task1 = Task.CreateNew("2020/12/30 test task 1", new DateTime(2020, 12, 30));
            Task task2 = Task.CreateNew("2020/11/30 test task 2", new DateTime(2020, 11, 30));
            Task task3 = Task.CreateNew("2020/10/20 test task 3", new DateTime(2020, 10, 20));

            Project p1 = Project.CreateNew("Test project 1");
            Project p2 = Project.CreateNew("Test project 2");

            task1.AttachToProject(p2);

            IList<Task> tasks = Task.GetWithoutProject();
            Assert.AreEqual(2, tasks.Count);
            Assert.IsTrue(tasks.Contains(task2));
            Assert.IsTrue(tasks.Contains(task3));
        }

        [TestCase]
        public void Note_CreateTask_NoteIsEmpty()
        {
            Task task1 = Task.CreateNew(
                "2020/12/30 test task 1", new DateTime(2020, 12, 30)
            );

            Assert.AreEqual("", task1.Note);
        }
        [TestCase]
        public void UpdateNotes_UpdateWithNull_ThrowsError()
        {
            Task task1 = Task.CreateNew(
                "2020/12/30 test task 1", new DateTime(2020, 12, 30)
            );
            var exception = Assert.Throws<ArgumentException>(
                () => task1.UpdateNote(null)
            );
            Assert.IsTrue(
                exception.Message.ToLower().Contains("note can't be null")
            );
        }
        [TestCase]
        public void UpdateNotes_UpdateWithNotNull_ValueUpdated()
        {
            Task task1 = Task.CreateNew(
                "2020/12/30 test task 1", new DateTime(2020, 12, 30)
            );

            task1.UpdateNote("new note");
            Assert.AreEqual("new note", task1.Note);
        }
        [TestCase]
        public void UpdateNotes_UpdateWithNotNull_UpdatecountIncrease()
        {
            Task task1 = Task.CreateNew(
                "2020/12/30 test task 1", new DateTime(2020, 12, 30)
            );
            int oldUpdCnt = tasks.UpdatesCount;
            task1.UpdateNote("new note");
            Assert.AreEqual(oldUpdCnt + 1, tasks.UpdatesCount);
        }
    }
}
