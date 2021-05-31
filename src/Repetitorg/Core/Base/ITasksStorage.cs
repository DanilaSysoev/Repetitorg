using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Base
{
    public interface ITasksStorage
    {
        void Save();
        void Load();
    }
}
