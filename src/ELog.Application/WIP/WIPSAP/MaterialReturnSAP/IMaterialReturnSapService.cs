using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.WIP.WIPSAP.MaterialReturnSAP.Dto;
using ELog.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.WIP.WIPSAP.MaterialReturnSAP
{
    public interface IMaterialReturnSapService : IApplicationService
    {
        Task<string> InsertMaterialReturnAsync(MaterialRteturnDetailsSAPDto input);
        Task<List<MaterialRteturnDetailsSAP>> GetMaterialReturnSAPDetailsList();

        Task DeleteAsync(EntityDto<int> input);
    }
}
