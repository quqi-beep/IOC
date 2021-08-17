using System;
using System.Collections.Generic;
using System.Text;

namespace CustomIoc.LifeTimeManager
{
    public interface ILifeTimeManager
    {
        object CreateInstance(Type type,params object[] args);
    }
}
