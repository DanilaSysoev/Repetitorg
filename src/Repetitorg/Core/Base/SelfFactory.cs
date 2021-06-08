using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core.Base
{
    public interface SelfFactory<T>
    {
        T CreateInstance(params object[] argumenst);
    }
}
