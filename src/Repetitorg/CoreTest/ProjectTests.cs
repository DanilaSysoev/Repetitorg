﻿using NUnit.Framework;
using Repetitorg.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.CoreTest
{
    [TestFixture]
    class ProjectTests
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
        public void Clear_ClearAfterThreeAddition_ProjectsCountEqualsZero()
        {
            Project.CreateNew("Test Project 1");
            Project.CreateNew("Test Project 2");
            Project.CreateNew("Test Project 3");

            Project.SetupStorage(new DummyStorage<Project>());

            Assert.AreEqual(0, Project.Count);
        }

        [TestCase]
        public void Add_SimpleAdd_ProjectsCountIncrease()
        {
            Project.CreateNew("Test Project");
            Assert.AreEqual(1, Project.Count);
        }
        [TestCase]
        public void Add_SimpleAdd_ProjectAddedToStorage()
        {
            int pastAC = projects.AddCount;
            Project.CreateNew("Test Project");
            Assert.AreEqual(pastAC + 1, projects.AddCount);
        }
        [TestCase]
        public void Add_AddThreeProjects_ProjectsCountEqualsThree()
        {
            Project.CreateNew("Test Project 1");
            Project.CreateNew("Test Project 2");
            Project.CreateNew("Test Project 3");

            Assert.AreEqual(3, Project.Count);
        }
        [TestCase]
        public void Add_AddWithDuplicateName_ThrowsException()
        {
            Project.CreateNew("Test Project");

            var exception = Assert.Throws<InvalidOperationException>(
                () => Project.CreateNew("Test Project")
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "project with name \"test project\" already exist"
            ));
        }
        [TestCase]
        public void Add_AddWithNullName_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => Project.CreateNew(null)
            );

            Assert.IsTrue(exception.Message.ToLower().Contains(
                "can't create project with null name"
            ));
        }

        [TestCase]
        public void GetAll_AddedThreeProjects_ReturnAll()
        {
            Project p1 = Project.CreateNew("Test Project 1");
            Project p2 = Project.CreateNew("Test Project 2");
            Project p3 = Project.CreateNew("Test Project 3");

            IReadOnlyList<Project> projects = Project.GetAll();

            Assert.AreEqual(3, projects.Count);
            Assert.IsTrue(projects.Contains(p1));
            Assert.IsTrue(projects.Contains(p2));
            Assert.IsTrue(projects.Contains(p3));
        }

        [TestCase]
        public void Remove_RemoveExisting_RemoveSuccess()
        {
            Project p1 = Project.CreateNew("Test Project 1");
            Project p2 = Project.CreateNew("Test Project 2");
            Project p3 = Project.CreateNew("Test Project 3");

            Project.Remove(p1);

            IReadOnlyList<Project> projects = Project.GetAll();

            Assert.AreEqual(2, projects.Count);
            Assert.IsFalse(projects.Contains(p1));
            Assert.IsTrue(projects.Contains(p2));
            Assert.IsTrue(projects.Contains(p3));
        }
        [TestCase]
        public void Remove_RemoveNonExistent_NothingHappens()
        {
            Project p1 = Project.CreateNew("Test Project 1");
            Project.SetupStorage(new DummyStorage<Project>());

            Project p2 = Project.CreateNew("Test Project 2");
            Project p3 = Project.CreateNew("Test Project 3");
            Project.Remove(p1);

            IReadOnlyList<Project> projects = Project.GetAll();

            Assert.AreEqual(2, projects.Count);
            Assert.IsFalse(projects.Contains(p1));
            Assert.IsTrue(projects.Contains(p2));
            Assert.IsTrue(projects.Contains(p3));
        }
        [TestCase]
        public void Remove_RemoveIfExistingTasks_ThrowsError()
        {
            Project p1 = Project.CreateNew("Test Project 1");

            Task t1 = Task.CreateNew("Test task 1", new DateTime(2022, 2, 1));
            t1.AttachToProject(p1);

            var exception = Assert.Throws<InvalidOperationException> (
                () => Project.Remove(p1)
            );

            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't remove project, exist attached tasks"
                )
            );
        }

        [TestCase]
        public void FindByName_FindByNull_ThrowsException()
        {
            Project p1 = Project.CreateNew("Test Project 1");
            Project p2 = Project.CreateNew("Oops Test Project 2");
            Project p3 = Project.CreateNew("OOPSTest Project 3");
            Project p4 = Project.CreateNew("Project 4");
            Project p5 = Project.CreateNew("Project 5");

            var exception = Assert.Throws<ArgumentException>(
                () => Project.FindByName(null)
            );
            Assert.IsTrue(exception.Message.ToLower().Contains(
                "filter pattern can't be null"
            ));

        }
        [TestCase]
        public void FindByName_FindThreeStrongOverlap_ReturnAll()
        {
            Project p1 = Project.CreateNew("Test Project 1");
            Project p2 = Project.CreateNew("Oops Test Project 2");
            Project p3 = Project.CreateNew("OOPSTest Project 3");
            Project p4 = Project.CreateNew("Project 4");
            Project p5 = Project.CreateNew("Project 5");

            List<Project> projects = Project.FindByName("Test");

            Assert.AreEqual(3, projects.Count);
            Assert.IsTrue(projects.Contains(p1));
            Assert.IsTrue(projects.Contains(p2));
            Assert.IsTrue(projects.Contains(p3));
        }
        [TestCase]
        public void FindByName_FindThreeCaseIgnoreOverlap_ReturnAll()
        {
            Project p1 = Project.CreateNew("test Project 1");
            Project p2 = Project.CreateNew("Oops TeSt Project 2");
            Project p3 = Project.CreateNew("OOPSTEST Project 3");
            Project p4 = Project.CreateNew("Project 4");
            Project p5 = Project.CreateNew("Project 5");

            List<Project> projects = Project.FindByName("test");

            Assert.AreEqual(3, projects.Count);
            Assert.IsTrue(projects.Contains(p1));
            Assert.IsTrue(projects.Contains(p2));
            Assert.IsTrue(projects.Contains(p3));
        }

        [TestCase]
        public void Complete_CompleteExisting_CompleteSuccess()
        {
            Project p1 = Project.CreateNew("Test Project 1");
            Project p2 = Project.CreateNew("Test Project 2");
            Project p3 = Project.CreateNew("Test Project 3");

            p1.Complete();

            Assert.IsTrue(p1.Completed);
            Assert.IsFalse(p2.Completed);
            Assert.IsFalse(p3.Completed);
        }
        [TestCase]
        public void Complete_CompleteExisting_CompletingUpdateProject()
        {
            Project p1 = Project.CreateNew("Test Project 1");
            Project p2 = Project.CreateNew("Test Project 2");
            Project p3 = Project.CreateNew("Test Project 3");

            Assert.AreEqual(0, projects.UpdatesCount);
            p1.Complete();
            Assert.AreEqual(1, projects.UpdatesCount);
        }
        [TestCase]
        public void Complete_CompleteWhenExistNonCompleteTasks_ThrowsError()
        {
            Project p1 = Project.CreateNew("Test Project 1");

            Task t1 = Task.CreateNew("Test task 1", new DateTime(2022, 2, 1));
            t1.AttachToProject(p1);

            var exception = Assert.Throws<InvalidOperationException>(
                () => p1.Complete()
            );

            Assert.IsTrue(
                exception.Message.ToLower().Contains(
                    "can't complete project, exist non-completed tasks"
                )
            );
        }
        [TestCase]
        public void Complete_CompleteWhenExistCompleteTasks_CompleteOk()
        {
            Project p1 = Project.CreateNew("Test Project 1");

            Task t1 = Task.CreateNew("Test task 1", new DateTime(2022, 2, 1));
            t1.AttachToProject(p1);
            t1.Complete();
            p1.Complete();

            Assert.IsTrue(p1.Completed);
        }

        [TestCase]
        public void UpdateNotes_UpdateWithNotNull_UpdatecountIncrease()
        {
            Project p1 = Project.CreateNew("Test Project 1");
            int oldUpdCnt = projects.UpdatesCount;
            p1.UpdateNote("new note");
            Assert.AreEqual(oldUpdCnt + 1, projects.UpdatesCount);
        }
    }
}
