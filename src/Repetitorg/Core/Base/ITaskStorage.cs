using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Base
{
    public interface ITaskStorage
    {
        void Save();
        void Load();
    }
}
