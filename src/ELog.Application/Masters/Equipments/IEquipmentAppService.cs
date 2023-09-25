using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.CommonDto;
using ELog.Application.Masters.Equipments.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Masters.Equipments
{
    public interface IEquipmentAppService : IApplicationService
    {
        Task<EquipmentDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<EquipmentListDto>> GetAllAsync(PagedEquipmentResultRequestDto input);

        Task<EquipmentDto> CreateAsync(CreateEquipmentDto input);

        Task<EquipmentDto> UpdateAsync(EquipmentDto input);

        Task DeleteAsync(EntityDto<int> input);

        Task ApproveOrRejectEquipmentAsync(ApprovalStatusDto input);
    }
}