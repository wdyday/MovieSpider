using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Core.Ioc
{
    public class Ioc
    {
        private static readonly UnityContainer _container;

        static Ioc()
        {
            _container = new UnityContainer();

            // 从配置文件中注册
            var unity = ConfigurationManager.GetSection("unity");
            if(unity != null)
            {
                _container.LoadConfiguration();
            }
        }

        /// <summary>
        /// 注册Service
        /// MVC: Global.OnApplicationStart中添加: Ioc.RegisterInheritedTypes(typeof(BaseService).Assembly, typeof(ServiceBase));
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="baseType"></param>
        public static void RegisterInheritedTypes(Assembly assembly, Type baseType)
        {
            _container.RegisterInheritedTypes(assembly, baseType);
        }

        public static void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            _container.RegisterType<TInterface, TImplementation>();
        }

        public static T Get<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
