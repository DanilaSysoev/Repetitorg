using NUnit.Framework;
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
    }
}
