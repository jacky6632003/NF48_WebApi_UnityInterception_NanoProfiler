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
            �w��NuGet�M��:
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

            #region ���k1: NanoProfilerAttribute + NanoProfilerCallHandler (���XAttribute�PICallHandler)

            /*
            �@��: �w����@��k�i�H���WAttribute�A�d�I�Ӥ�k�A�b�����k�e�Τ���Q�n����Y�q�{���C
            �B�J:
            1.�إ� NanoProfilerCallHandler
            2.�إ� NanoProfilerAttribute
            3.�b�Q�n���� ICallHandler��Invoke ���a��A���W[NanoProfiler]
            4.�]�wUnityConfig.cs��RegisterTypes �]�w�d�I���A�w�靈���W[NanoProfiler]����k�A�����O�Ӱ����U
            */

            /*
            //���U��@���O ICustomerService
            container.AddNewExtension<Interception>(); //�n�����U
            container.RegisterType<ICustomerService, CustomerService>()
                .Configure<Interception>()
                .SetInterceptorFor<ICustomerService>(new InterfaceInterceptor());

            //���U��@���O ICustomerRepository
            //container.AddNewExtension<Interception>(); //�g�@���N�n
            container.RegisterType<ICustomerRepository, CustomerRepository>()
                .Configure<Interception>()
                .SetInterceptorFor<ICustomerRepository>(new InterfaceInterceptor());
            */

            /*
            //���U�h�����O
            //�n�w�� Unity.RegistrationByConvention�A�~���X�R��kRegisterTypes
            //�w��Ҧ��ե�W�ٵ����OService��Repository�����O���n���U
            container.AddNewExtension<Interception>(); //�n�����U
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

            #endregion ���k1: NanoProfilerAttribute + NanoProfilerCallHandler (���XAttribute�PICallHandler)

            #region ���k2: NanoProfilerInterceptionBehavior (IInterceptionBehavior)

            /*
            �@��: �w����@���O�A�d�I��Ҧ���k�A�b�����k�e�Τ���Q�n����Y�q�{���C
            �B�J:
            1.�إ� NanoProfilerInterceptionBehavior
            2.�]�wUnityConfig.cs��RegisterTypes �]�w�d�I���A�n���檺���O�����U
            */

            /*
            //���U��@���O ICustomerService
            container.AddNewExtension<Interception>(); //�n�����U
            container.RegisterType<ICustomerService, CustomerService>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<NanoProfilerInterceptionBehavior>()
            );

            //���U��@���O ICustomerRepository
            //container.AddNewExtension<Interception>(); //�g�@���N�n
            container.RegisterType<ICustomerRepository, CustomerRepository>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<NanoProfilerInterceptionBehavior>()
            );
            */

            
            //���U�h�����O
            //�n�w�� Unity.RegistrationByConvention�A�~���X�R��kRegisterTypes
            //�w��Ҧ��ե�W�ٵ����OService��Repository�����O���n���U
            container.AddNewExtension<Interception>(); //�n�����U
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
            

            #endregion ���k2: NanoProfilerInterceptionBehavior (IInterceptionBehavior)
        }
    }
}