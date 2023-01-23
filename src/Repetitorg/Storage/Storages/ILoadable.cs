using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    interface ILoadable
    {
        void Load(string pathToDb);
    }
}
