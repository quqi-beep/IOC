using CustomIoc.LifeTimeManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomIoc
{
    public class RegisteredType
    {
        public Type TargetType { get; set; }

        public ILifeTimeManager LifeTimeManager { get; set; }
    }

    public static class RegisteredTypeExtension
    {
        public static object CreateInstance(this RegisteredType type, params object[] args)
        {
            //使用生命周期管理创建实例
            return type.LifeTimeManager.CreateInstance(type.TargetType, args);
        }
    }
}
