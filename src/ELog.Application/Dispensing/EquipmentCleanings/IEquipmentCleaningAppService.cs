using Abp.Application.Services;

using ELog.Application.Dispensing.EquipmentCleanings.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Dispensing.EquipmentCleanings
{
    public interface IEquipmentCleaningAppService : IApplicationService
    {
        Task<EquipmentCleaningTransactionDto> CreateAsync(CreateEquipmentCleaningTransactionDto input);

        Task UpdateAsync(EquipmentCleaningTransactionDto input);

        Task<EquipmentCleaningTransactionDto> ValidateEquipmentStatusAsync(int equipmentId, int type, bool isSampling);
    }
}