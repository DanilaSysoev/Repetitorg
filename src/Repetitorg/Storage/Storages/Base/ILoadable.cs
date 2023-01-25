using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages.Base
{
    interface ILoadable
    {
        void Load(string pathToDb);
    }
}
