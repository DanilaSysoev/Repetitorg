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
    }
}
