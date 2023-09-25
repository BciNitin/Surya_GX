using Abp.Application.Services;

using ELog.Application.WIP.WIPLineClearances.Dto;

using System.Threading.Tasks;

namespace ELog.Application.WIP.WIPLineClearances
{
    public interface IWIPLineClearanceAppService : IApplicationService
    {
        Task<WIPLineClearanceTransactionDto> CreateAsync(CreateWIPLineClearanceDto input);

        Task UpdateAsync(WIPLineClearanceTransactionDto input);
    }
}
