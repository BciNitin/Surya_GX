using Abp.Application.Services.Dto;

using ELog.Application.Inward.WeightCaptures.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Inward.WeightCaptures
{
    public interface IWeightCaptureAppService
    {
        Task DeleteWeightCaptureDetailsAsync(EntityDto<int> input);

        Task<WeightCaptureDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<WeightCaptureListDto>> GetAllAsync(PagedWeightCaptureResultRequestDto input);

        Task<WeightCaptureDto> CreateAsync(CreateWeightCaptureDto input);

        Task<WeightCaptureDetailsDto> InsertWeightCaptureDetail(WeightCaptureDetailsDto input);
        Task<int?> IsWeightCapturePresent(int? PurchaseOrderId, int? InvoiceId, int? MaterialId, int? MfgBatchNoId);
    }
}