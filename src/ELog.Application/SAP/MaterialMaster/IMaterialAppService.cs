using Abp.Application.Services;

using ELog.Application.SAP.MaterialMaster.Dto;

using System.Threading.Tasks;

namespace ELog.Application.SAP.MaterialMaster
{
    public interface IMaterialAppService : IApplicationService
    {
        Task CreateAsync(SAPMaterial input);

        Task<SAPMaterial> GetAsync(string materialCode);
    }
}