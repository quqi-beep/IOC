using CustomIoc.LifeTimeManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CustomIoc
{
    public class MyContainer : IMyContainer
    {
        //键用来映射关系，使用字典作为登记本
        private static readonly Dictionary<string, RegisteredType> iocDictionary = new Dictionary<string, RegisteredType>();

        public void RegisterType<TType>()
        {
            RegisterType<TType, TType>("");
        }

        public void RegisterType<TType>(string name)
        {
            RegisterType<TType, TType>("");
        }

        public void RegisterType<TType, TImplementation>()
        {
            RegisterType<TType, TImplementation>("");
        }

        public void RegisterType<TType, TImplementation>(string name)
        {
            //string key = typeof(TType).FullName;
            //if (!string.IsNullOrWhiteSpace(name))
            //{
            //    key = name;
            //}

            //if (iocDictionary.ContainsKey(key))
            //{
            //    //如果存在采取覆盖形式
            //    iocDictionary[key] = typeof(TImplementation);
            //}
            //else
            //{
            //    //将传进来的泛型Type进行关系映射
            //    iocDictionary.Add(key, typeof(TImplementation));
            //}
            RegisterType<TType, TImplementation>(name, TypeLifeTime.Transient);
        }

        public void RegisterType<TType>(ILifeTimeManager lifeTimeManager)
        {
            RegisterType<TType, TType>(lifeTimeManager);
        }

        public void RegisterType<TType>(string name, ILifeTimeManager lifeTimeManager)
        {
            RegisterType<TType, TType>(name, lifeTimeManager);
        }

        public void RegisterType<TType, TImplementation>(ILifeTimeManager lifeTimeManager)
        {
            RegisterType<TType, TImplementation>("", lifeTimeManager);
        }

        public void RegisterType<TType, TImplementation>(string name, ILifeTimeManager lifeTimeManager)
        {
            string key = typeof(TType).FullName;
            if (!string.IsNullOrWhiteSpace(name))
            {
                key = name;
            }

            if (iocDictionary.ContainsKey(key))
            {
                //如果存在覆盖
                iocDictionary[key] = new RegisteredType
                {
                    TargetType = typeof(TImplementation),
                    LifeTimeManager = lifeTimeManager
                };
            }
            else
            {
                //将传进来的泛型Type进行关系映射
                iocDictionary.Add(key, new RegisteredType
                {
                    TargetType = typeof(TImplementation),
                    LifeTimeManager = lifeTimeManager,
                });
            }
        }

        public TType Resolve<TType>()
        {
            return Resolve<TType>(null);
        }

        public TType Resolve<TType>(string name)
        {
            //解析泛型Type获取key
            var key = typeof(TType).FullName;
            if (!string.IsNullOrWhiteSpace(name))
            {
                key = name;
            }
            var type = iocDictionary[key];

            //创建一个对象实例
            //return (TType)Activator.CreateInstance(type);
            return (TType)CreateInstance(type);
        }


        private object CreateInstance(RegisteredType type)
        {
            //获取所有的构造函数
            var ctors = type.TargetType.GetConstructors();
            //获取被标记的构造函数
            var ctor = ctors.FirstOrDefault(t => t.IsDefined(typeof(MyinjectionAttribute), true));
            if (ctor != null)
            {
                CreateInstance(type, ctor);
            }

            ctor = ctors.OrderByDescending(t => t.GetParameters().Length).First();
            return CreateInstance(type, ctor);
        }


        private object CreateInstance(RegisteredType type, ConstructorInfo ctor)
        {
            //获取构造函数参数
            var paraArray = ctor.GetParameters();
            if (paraArray.Length == 0)
            {
                return type.CreateInstance();
            }

            var parameters = new List<object>();
            foreach (var para in paraArray)
            {
                //通过反射获取参数类型名
                var paraKey = para.ParameterType.FullName;
                //根据paraKey从字典获取已注册的实现类
                var paraType = iocDictionary[paraKey];

                //递归注入构造函数参数
                var paraObj = CreateInstance(paraType);
                // 将对象存储在list数组
                parameters.Add(paraObj);
            }

            //调用生命周期管理器创建时间方法
            //type.LifeTimeManager.CreateInstance(type, parameters.ToArray());

            //这里使用扩展类调用生命周期管理器创建时间方法
            return type.CreateInstance(parameters.ToArray());
        }
    }
}
