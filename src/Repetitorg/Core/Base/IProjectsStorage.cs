using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core.Base
{
    public interface IProjectsStorage
    {
        void Save(List<Project> projects);
        List<Project> Load();
    }
}
