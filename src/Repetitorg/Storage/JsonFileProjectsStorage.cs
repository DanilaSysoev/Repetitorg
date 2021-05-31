using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Storage
{
    public class JsonFileProjectsStorage : IProjectsStorage
    {
        private string path;

        public JsonFileProjectsStorage(string path)
        {
            this.path = path;
        }
    }
}
