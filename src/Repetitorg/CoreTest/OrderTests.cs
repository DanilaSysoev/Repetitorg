﻿using NUnit.Framework;
using Repetitorg.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repetitorg.CoreTest
{
    [TestFixture]
    class OrderTests
    {
        [TearDown]
        public void Clear()
        {
            Order.Clear();
            Client.Clear();
            Student.Clear();
        }
        [SetUp]
        public void Initialize()
        {
            Client.CreateNew("c1");
            Client.CreateNew("c2");
            Client.CreateNew("c3");

            Student.CreateNew("s1");
            Student.CreateNew("s2");
            Student.CreateNew("s3");
        }

    }
}
