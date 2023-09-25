using Abp.Application.Services;

using ELog.Application.SAP.ProcessOrder.Dto;

using System.Threading.Tasks;

namespace ELog.Application.SAP.ProcessOrder
{
    public interface IProcessOrderAppService : IApplicationService
    {
        Task<string> UpdateAsync(ProcessOrderDto input);
    }
}