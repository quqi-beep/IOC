using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CustomIoc.LifeTimeManager
{
    /// <summary>
    /// 线程单例模式（作用域模式）
    /// </summary>
    public class PreThreadLifeTimeManger : ILifeTimeManager
    {
        //通过线程本地存储进行线程单例管理
        private readonly ThreadLocal<Dictionary<string, object>> threadLocalDictionary = new ThreadLocal<Dictionary<string, object>>();
        public object CreateInstance(Type type, params object[] args)
        {
            if (threadLocalDictionary.Value == null)
            {
                threadLocalDictionary.Value = new Dictionary<string, object>();
            }

            var key = type.FullName;
            if (threadLocalDictionary.Value.ContainsKey(key))
            {
                return threadLocalDictionary.Value[key];
            }

            var instance = Activator.CreateInstance(type, args);
            threadLocalDictionary.Value.Add(key, instance);
            return instance;
        }
    }
}
