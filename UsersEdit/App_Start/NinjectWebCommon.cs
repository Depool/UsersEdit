[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(UsersEdit.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(UsersEdit.App_Start.NinjectWebCommon), "Stop")]

namespace UsersEdit.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using ApplicationRepository.Interface;
    using ApplicationRepository.Concrete.ADOSql;
    using ApplicationRepository.Concrete.Entity;
    using Ninject.Modules;
    using Ninject.Extensions.Factory;
    using Infrastructure.Logging.Concrete.Nlog;
    using Infrastructure.Logging.Interface;
   // using Infrastructure.

    public interface IRepositoryFactory
    {
        IUserRepository CreateUserRepository();
        IImageRepository CreateImageRepository();
        IRoleRepository CreateRoleRepository();
        IMailMessageRepository CreateMailMessageRepository();
    }

    public class RepositoriesModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<IRoleRepository>().To<RoleRepository>();
            //Bind<IUserRepository>().To<UserRepository>();
            //Bind<IImageRepository>().To<ImageRepository>();
            //Bind<IMailMessageRepository>().To<MailMessageRepository>();

            Bind<IRoleRepository>().To<ADOSqlRoleRepository>();
            Bind<IUserRepository>().To<ADOSqlUserRepository>();
            Bind<IImageRepository>().To<ADOSqlImageRepository>();
            Bind<IMailMessageRepository>().To<ADOSqlMailMessageRepository>();
            
            Bind<IRepositoryFactory>().ToFactory();
        }
    }

    public interface ILoggerFactory
    {
        ILogger CreateDefaultLogger();
    }

    public class LoggerModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<NLogLogger>();

            Bind<ILoggerFactory>().ToFactory();
        }
    }

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));

            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel(new RepositoriesModule());
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {

        }        
    }
}
