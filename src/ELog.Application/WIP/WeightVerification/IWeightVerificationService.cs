using Abp.Application.Services.Dto;
using ELog.Application.SelectLists.Dto;
using ELog.Application.WIP.WeightVerification.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.WIP.WeightVerification
{
    public interface IWeightVerificationService
    {
        Task<WeightVerificationDto> CreateAsync(CreateWeightVerificationDto input);
        //Task<WeightVerificationDetailDto> InsertWeightVerificationDetail(WeightVerificationDetailDto input);
        Task<WeightVerificationDto> GetAsync(EntityDto<int> input);
        Task<List<SelectListDto>> GetDispenseBarcode(string input);
        Task<WeightVerificationDto> GetDispenseDetailsrAsync(int input);
        Task<WeightVerificationDto> UpdateAsync(WeightVerificationDto input);
        Task<List<SelectListDto>> GetProcessOrdersOfProductCodeAsync(string input);
        Task<WeightVerificationDto> GetBatchDetailsOfProcessOrderAsync(int input);
        Task<PagedResultDto<WeightVerificationListDto>> GetAllAsync(PagedWeightVrifyReturnRequestDto input);
        Task<WeightVerificationDto> GetWeightVerificationfromDispenseIdAsync(int input);
        Task<SelectListDto> GetDispenseBarcodefromIdAsync(int input);
    }
}
