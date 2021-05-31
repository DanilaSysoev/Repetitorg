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
    }
}
