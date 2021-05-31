using NUnit.Framework;
using Repetitorg.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repetitorg.StorageTest
{
    [TestFixture]
    class JsonFileProjectsStorageTests
    {
        [TearDown]
        public void Clear()
        {
            Project.Clear();
            var storage = new JsonFileProjectsStorage(TEST_DATA_PATH);
            Project.Save(storage);
        }

        private const string TEST_DATA_PATH = "D:\\YandexDisk\\YandexDisk\\Danila\\Work\\Repetitorg";
    }
}
