using Abp.Modules;
using Abp.Reflection.Extensions;
using ELog.Web.Core;
using ELog.Web.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ELog.Web.Host.Startup
{
    [DependsOn(
       typeof(PMMSWebCoreModule))]
    public class PMMSWebHostModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public PMMSWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PMMSWebHostModule).GetAssembly());
        }
    }
}