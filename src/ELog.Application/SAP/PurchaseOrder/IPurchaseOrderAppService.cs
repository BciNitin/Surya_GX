using Abp.Application.Services;

using ELog.Application.SAP.PurchaseOrder.Dto;

using System.Threading.Tasks;

namespace ELog.Application.SAP.PurchaseOrder
{
    public interface IPurchaseOrderAppService : IApplicationService
    {
        Task<string> UpdateAsync(PurchaseOrderDto input);
    }
}