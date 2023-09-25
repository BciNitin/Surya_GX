using Abp.Dependency;
using Castle.Windsor.MsDependencyInjection;
using ELog.Core.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ELog.Migrator.DependencyInjection
{
    public static class ServiceCollectionRegistrar
    {
        public static void Register(IIocManager iocManager)
        {
            var services = new ServiceCollection();

            IdentityRegistrar.Register(services);

            WindsorRegistrationHelper.CreateServiceProvider(iocManager.IocContainer, services);
        }
    }
}
