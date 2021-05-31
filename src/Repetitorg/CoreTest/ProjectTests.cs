using NUnit.Framework;
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
        public void Add_SimpleAdd_ProjectsCountIncrease()
        {
            Project.Add("Test Project");
            Assert.AreEqual(1, Project.ProjectsCount);
        }
    }
}
