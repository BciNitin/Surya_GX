using Abp.Application.Services;

using ELog.Application.SAP.Dto;
using ELog.Application.SAP.MaterialMaster.Dto;

using System.Threading.Tasks;

namespace ELog.Application.SAP
{
    public interface ISAPIntegrationAppService : IApplicationService
    {
        Task InsertUpdateMaterialAsync(SAPMaterial input);

        Task<SAPMaterial> GetMaterialAsync(string materialCode);

        Task InsertUpdateProcessOrderAsync(SAPProcessOrderDto input);

        Task InsertUpdateReturnMaterialAsync(SAPReturntoMaterialDto input);

        Task<string> InsertUpdatePurchaseOrderMaterial(SAPProcessOrderReceivedMaterialDto input);

        Task<string> InsertUpdateQualityControlDetail(SAPQualityControlDetailDto input);

        Task InsertUpdatePlantMasterAsync(SAPPlantMasterDto input);

        Task InsertUpdateUomMasterAsync(SAPUOMMasterDto input);
    }
}