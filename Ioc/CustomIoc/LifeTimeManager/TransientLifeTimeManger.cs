using System;
using System.Collections.Generic;
using System.Text;

namespace CustomIoc.LifeTimeManager
{
    /// <summary>
    /// 瞬时模式
    /// </summary>
    public class TransientLifeTimeManger : ILifeTimeManager
    {
        public object CreateInstance(Type type, params object[] args)
        {
            return Activator.CreateInstance(type, args);
        }
    }
}
