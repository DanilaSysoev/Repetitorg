using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repetitorg.Core;

namespace CoreTest
{
    [TestFixture]
    class ClientTest
    {
        [TestCase]
        public void CreateNew_CreateSimpleClient_IncrementClientsCount()
        {
            Client.CreateNew("Иванов Иван Иванович");
            Assert.AreEqual(1, Client.ClientsCount);
        }
    }
}
