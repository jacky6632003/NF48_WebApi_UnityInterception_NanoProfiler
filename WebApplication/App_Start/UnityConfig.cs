using System;
using System.Linq;
using Business.IServices;
using Business.Services;
using DataAccess.Helpers;
using DataAccess.IRepositories;
using DataAccess.Repositories;
using Unity;
using Unity.Injection;
using Unity.Interception;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;
using Unity.RegistrationByConvention;
using WebApplication.Controllers;
using WebApplication.Helpers;
using WebApplication.InterceptionBehaviors;

namespace WebApplication
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            /*
            安裝NuGet套件:
            1.Unity
            2.Unity.AspNet.WebApi
            3.Unity.Interception
            4.Unity.RegistrationByConvention
            5.NanoProfiler.Web
            6.NanoProfiler.Data
            */

            // TODO: Register your type's mappings here.

            container.RegisterType<IConnectionStringHelper, ConnectionStringHelper>();
            container.RegisterType<IConnectionHelper, ConnectionHelper>();

            //container.RegisterType<ICustomerService, CustomerService>();
            //container.RegisterType<ICustomerRepository, CustomerRepository>();

            #region 做法1: NanoProfilerAttribute + NanoProfilerCallHandler (結合Attribute與ICallHandler)

            /*
            作用: 針對任一方法可以掛上Attribute，攔截該方法，在執行方法前或之後想要執行某段程式。
            步驟:
            1.建立 NanoProfilerCallHandler
            2.建立 NanoProfilerAttribute
            3.在想要執行 ICallHandler的Invoke 的地方，掛上[NanoProfiler]
            4.設定UnityConfig.cs的RegisterTypes 設定攔截器，針對有掛上[NanoProfiler]的方法，其類別來做註冊
            */

            /*
            //註冊單一類別 ICustomerService
            container.AddNewExtension<Interception>(); //要先註冊
            container.RegisterType<ICustomerService, CustomerService>()
                .Configure<Interception>()
                .SetInterceptorFor<ICustomerService>(new InterfaceInterceptor());

            //註冊單一類別 ICustomerRepository
            //container.AddNewExtension<Interception>(); //寫一次就好
            container.RegisterType<ICustomerRepository, CustomerRepository>()
                .Configure<Interception>()
                .SetInterceptorFor<ICustomerRepository>(new InterfaceInterceptor());
            */

            /*
            //註冊多個類別
            //要安裝 Unity.RegistrationByConvention，才有擴充方法RegisterTypes
            //針對所有組件名稱結尾是Service或Repository的類別都要註冊
            container.AddNewExtension<Interception>(); //要先註冊
            container.RegisterTypes
                (
                    AllClasses.FromLoadedAssemblies()
                        .Where(x => x.Name.EndsWith("Service") || x.Name.EndsWith("Repository")),
                    WithMappings.FromMatchingInterface
                )
                .Configure<Interception>()
                .SetInterceptorFor<ICustomerService>(new InterfaceInterceptor())
                .SetInterceptorFor<ICustomerRepository>(new InterfaceInterceptor());
            */

            #endregion 做法1: NanoProfilerAttribute + NanoProfilerCallHandler (結合Attribute與ICallHandler)

            #region 做法2: NanoProfilerInterceptionBehavior (IInterceptionBehavior)

            /*
            作用: 針對任一類別，攔截其所有方法，在執行方法前或之後想要執行某段程式。
            步驟:
            1.建立 NanoProfilerInterceptionBehavior
            2.設定UnityConfig.cs的RegisterTypes 設定攔截器，要執行的類別做註冊
            */

            /*
            //註冊單一類別 ICustomerService
            container.AddNewExtension<Interception>(); //要先註冊
            container.RegisterType<ICustomerService, CustomerService>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<NanoProfilerInterceptionBehavior>()
            );

            //註冊單一類別 ICustomerRepository
            //container.AddNewExtension<Interception>(); //寫一次就好
            container.RegisterType<ICustomerRepository, CustomerRepository>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<NanoProfilerInterceptionBehavior>()
            );
            */

            
            //註冊多個類別
            //要安裝 Unity.RegistrationByConvention，才有擴充方法RegisterTypes
            //針對所有組件名稱結尾是Service或Repository的類別都要註冊
            container.AddNewExtension<Interception>(); //要先註冊
            container.RegisterTypes(
                AllClasses.FromLoadedAssemblies()
                    .Where(x => x.Name.EndsWith("Service") || x.Name.EndsWith("Repository")),
                WithMappings.FromMatchingInterface,
                WithName.Default,
                overwriteExistingMappings: true,
                getInjectionMembers: t => new InjectionMember[]
                {
                    new Interceptor<InterfaceInterceptor>(),
                    new InterceptionBehavior<NanoProfilerInterceptionBehavior>(),
                }
            );
            

            #endregion 做法2: NanoProfilerInterceptionBehavior (IInterceptionBehavior)
        }
    }
}