using NUnit.Framework;
using Repetitorg.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreTest
{
    [TestFixture]
    class ProjectTests
    {
        [TearDown]
        public void Clear()
        {
            Project.Clear();
            Task.Clear();
        }


        [TestCase]
        public void Clear_ClearAfterThreeAddition_ProjectsCountEqualsZero()
        {
            Project.Add("Test Project 1");
            Project.Add("Test Project 2");
            Project.Add("Test Project 3");

            Project.Clear();

            Assert.AreEqual(0, Project.ProjectsCount);
        }

        [TestCase]
        public void Add_SimpleAdd_ProjectsCountIncrease()
        {
            Project.Add("Test Project");
            Assert.AreEqual(1, Project.ProjectsCount);
        }
        [TestCase]
        public void Add_AddThreeProjects_ProjectsCountEqualsThree()
        {
            Project.Add("Test Project 1");
            Project.Add("Test Project 2");
            Project.Add("Test Project 3");

            Assert.AreEqual(3, Project.ProjectsCount);
        }
        [TestCase]
        public void Add_AddWithDuplicateName_ThrowsException()
        {
            Project.Add("Test Project");

            var exception = Assert.Throws<InvalidOperationException>(() => Project.Add("Test Project"));

            Assert.IsTrue(exception.Message.Contains(
                "Project with name \"Test Project\" already exist"
            ));
        }
        [TestCase]
        public void Add_AddWithNullName_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => Project.Add(null)
            );

            Assert.IsTrue(exception.Message.Contains(
                "Can't create project with NULL name"
            ));
        }

        [TestCase]
        public void GetAll_AddedThreeProjects_ReturnAll()
        {
            Project p1 = Project.Add("Test Project 1");
            Project p2 = Project.Add("Test Project 2");
            Project p3 = Project.Add("Test Project 3");

            List<Project> projects = Project.GetAll();

            Assert.AreEqual(3, projects.Count);
            Assert.IsTrue(projects.Contains(p1));
            Assert.IsTrue(projects.Contains(p2));
            Assert.IsTrue(projects.Contains(p3));
        }
        [TestCase]
        public void GetAll_AddedThreeProjects_ReturnCopyOfCollection()
        {
            Project p1 = Project.Add("Test Project 1");
            Project p2 = Project.Add("Test Project 2");
            Project p3 = Project.Add("Test Project 3");

            List<Project> projects = Project.GetAll();
            projects.Remove(p1);
            List<Project> projectsOld = Project.GetAll();

            Assert.AreEqual(3, projectsOld.Count);
            Assert.IsTrue(projectsOld.Contains(p1));
            Assert.IsTrue(projectsOld.Contains(p2));
            Assert.IsTrue(projectsOld.Contains(p3));
        }

        [TestCase]
        public void Remove_RemoveExisting_RemoveSuccess()
        {
            Project p1 = Project.Add("Test Project 1");
            Project p2 = Project.Add("Test Project 2");
            Project p3 = Project.Add("Test Project 3");

            Project.Remove(p1);

            List<Project> projects = Project.GetAll();

            Assert.AreEqual(2, projects.Count);
            Assert.IsFalse(projects.Contains(p1));
            Assert.IsTrue(projects.Contains(p2));
            Assert.IsTrue(projects.Contains(p3));
        }
        [TestCase]
        public void Remove_RemoveNonExistent_NothingHappens()
        {
            Project p1 = Project.Add("Test Project 1");
            Project.Clear();

            Project p2 = Project.Add("Test Project 2");
            Project p3 = Project.Add("Test Project 3");
            Project.Remove(p1);

            List<Project> projects = Project.GetAll();

            Assert.AreEqual(2, projects.Count);
            Assert.IsFalse(projects.Contains(p1));
            Assert.IsTrue(projects.Contains(p2));
            Assert.IsTrue(projects.Contains(p3));
        }

        [TestCase]
        public void FindByName_FindByNull_ThrowsException()
        {
            Project p1 = Project.Add("Test Project 1");
            Project p2 = Project.Add("Oops Test Project 2");
            Project p3 = Project.Add("OOPSTest Project 3");
            Project p4 = Project.Add("Project 4");
            Project p5 = Project.Add("Project 5");

            var exception = Assert.Throws<ArgumentException>(
                () => Project.FindByName(null)
            );
            Assert.IsTrue(exception.Message.Contains(
                "Filter pattern can't be null"
            ));

        }
        [TestCase]
        public void FindByName_FindThreeStrongOverlap_ReturnAll()
        {
            Project p1 = Project.Add("Test Project 1");
            Project p2 = Project.Add("Oops Test Project 2");
            Project p3 = Project.Add("OOPSTest Project 3");
            Project p4 = Project.Add("Project 4");
            Project p5 = Project.Add("Project 5");

            List<Project> projects = Project.FindByName("Test");

            Assert.AreEqual(3, projects.Count);
            Assert.IsTrue(projects.Contains(p1));
            Assert.IsTrue(projects.Contains(p2));
            Assert.IsTrue(projects.Contains(p3));
        }
        [TestCase]
        public void FindByName_FindThreeCaseIgnoreOverlap_ReturnAll()
        {
            Project p1 = Project.Add("test Project 1");
            Project p2 = Project.Add("Oops TeSt Project 2");
            Project p3 = Project.Add("OOPSTEST Project 3");
            Project p4 = Project.Add("Project 4");
            Project p5 = Project.Add("Project 5");

            List<Project> projects = Project.FindByName("test");

            Assert.AreEqual(3, projects.Count);
            Assert.IsTrue(projects.Contains(p1));
            Assert.IsTrue(projects.Contains(p2));
            Assert.IsTrue(projects.Contains(p3));
        }

        [TestCase]
        public void Complete_CompleteExisting_CompleteSuccess()
        {
            Project p1 = Project.Add("Test Project 1");
            Project p2 = Project.Add("Test Project 2");
            Project p3 = Project.Add("Test Project 3");

            Project.Complete(p1);

            Assert.IsTrue(p1.Completed);
            Assert.IsFalse(p2.Completed);
            Assert.IsFalse(p3.Completed);
        }
        [TestCase]
        public void Complete_CompleteAfterRemoveAndRestore_CompleteSuccess()
        {
            Project p1 = Project.Add("Test Project 1");
            Project p2 = Project.Add("Test Project 2");
            Project p3 = Project.Add("Test Project 3");

            Project.Remove(p1);
            Project p1new = Project.Add("Test Project 1");

            Project.Complete(p1);

            Assert.IsTrue(p1.Completed);
            Assert.IsTrue(p1new.Completed);
            Assert.IsFalse(p2.Completed);
            Assert.IsFalse(p3.Completed);
        }
    }
}
