using Internship.Models.DAL;
using Internship.Models.Domain;
using Internship.Models.DAL;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Internship.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Internship.App_Start.NinjectWebCommon), "Stop")]

namespace Internship.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

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
            var kernel = new StandardKernel();
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
            kernel.Bind<IUserRepository>().To<UserRepository>().InRequestScope();
            kernel.Bind<IStudentRepository>().To<StudentRepository>().InRequestScope();
            kernel.Bind<IStagebegeleiderRepository>().To<StagebegeleiderRepository>().InRequestScope();
            kernel.Bind<IBedrijfRepository>().To<BedrijfRepository>().InRequestScope();
            kernel.Bind<InternshipContext>().ToSelf().InRequestScope();
            kernel.Bind<ISpecialisatieRepository>().To<SpecialisatieRepository>().InRequestScope();
            kernel.Bind<IOpdrachtRepository>().To<OpdrachtenRepository>().InRequestScope();
            kernel.Bind<IGemeenteRepository>().To<GemeenteRepository>().InRequestScope();
            kernel.Bind<IContactPersoonRepository>().To<ContactPersoonRepository>().InRequestScope();
            kernel.Bind<IStatusRepository>().To<StatusRepository>().InRequestScope();

        }        
    }
}
