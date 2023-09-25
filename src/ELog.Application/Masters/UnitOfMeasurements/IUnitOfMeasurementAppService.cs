using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.CommonDto;
using ELog.Application.Masters.UnitOfMeasurements.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Masters.UnitOfMeasurements
{
    public interface IUnitOfMeasurementAppService : IApplicationService
    {
        Task<UnitOfMeasurementDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<UnitOfMeasurementListDto>> GetAllAsync(PagedUnitOfMeasurementResultRequestDto input);

        Task<UnitOfMeasurementDto> CreateAsync(CreateUnitOfMeasurementDto input);

        Task<UnitOfMeasurementDto> UpdateAsync(UnitOfMeasurementDto input);

        Task DeleteAsync(EntityDto<int> input);
        Task ApproveOrRejectUnitOfMeasurementAsync(ApprovalStatusDto input);
    }
}