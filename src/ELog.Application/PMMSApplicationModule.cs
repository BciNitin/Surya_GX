using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.Ldap;

using ELog.Adapter;
using ELog.Adapter.SAPAjantaAdapter;
using ELog.Connector;
using ELog.Connector.Weighing;
using ELog.ConnectorFactory;
using ELog.Core;
using ELog.Core.Authorization;
using ELog.ERPConnector;
using ELog.HardwareConnector.Printer;
using ELog.HardwareConnectorFactory;

namespace ELog.Application
{
    [DependsOn(
        typeof(PMMSCoreModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpZeroLdapModule))]
    public class PMMSApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<PMMSAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(PMMSApplicationModule).GetAssembly();
            IocManager.Register<ERPConnectorFactory.ERPConnectorFactory, ERPConnectorFactory.ERPConnectorFactory>();
            IocManager.Register<IConnector, SAPAjantaConnector>();
            IocManager.Register<IHttpClientProvider, HttpClientProvider>();

            IocManager.Register<ISAPAjantaAdapter, SAPAjantaAdapter>();
            IocManager.Register<IPrinterConnector, PRNPrinter>();
            IocManager.Register<PrinterFactory, PrinterFactory>();
            IocManager.Register<IWeighingScaleConnector, WeighingScale>();
            IocManager.Register<WeighingScaleFactory, WeighingScaleFactory>();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}