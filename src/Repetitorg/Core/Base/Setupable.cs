using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core.Base
{
    abstract class Setupable<T>
    {
        internal abstract void Setup(params object[] argumenst);
    }
}
