using System;
using System.Collections.Generic;
using System.Text;

namespace CustomIoc.LifeTimeManager
{
    public class TypeLifeTime
    {
        public static ILifeTimeManager Singleton = new SingletonLifeTimeManager();
        public static ILifeTimeManager Transient = new TransientLifeTimeManger();
        public static ILifeTimeManager PerThread = new PreThreadLifeTimeManger();
    }
}
