using Abp.Application.Services;

using ELog.Application.Dispensing.CubicleCleanings.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Dispensing.CubicleCleanings
{
    public interface ICubicleCleaningAppService : IApplicationService
    {
        Task<CubicleCleaningTransactionDto> CreateAsync(CreateCubicleCleaningTransactionDto input);
        Task<CubicleCleaningTransactionDto> CreateSamplingAsync(CreateCubicleCleaningTransactionDto input);
        Task UpdateAsync(CubicleCleaningTransactionDto input);
        Task UpdateSamplingAsync(CubicleCleaningTransactionDto input);
        Task<CubicleCleaningTransactionDto> ValidateCubicleStatusAsync(int cubicleId, int type, bool isSampling);
    }
}