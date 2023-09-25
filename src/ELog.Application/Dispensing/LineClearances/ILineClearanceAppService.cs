using Abp.Application.Services;

using ELog.Application.Dispensing.LineClearances.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Dispensing.LineClearances
{
    public interface ILineClearanceAppService : IApplicationService
    {
        Task<LineClearanceTransactionDto> CreateAsync(CreateLineClearanceTransactionDto input);

        Task UpdateAsync(LineClearanceTransactionDto input);
    }
}