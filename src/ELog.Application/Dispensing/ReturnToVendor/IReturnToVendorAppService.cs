using Abp.Application.Services;

using ELog.Application.CommonDto;
using ELog.Application.Dispensing.ReturnToVendor.Dto;

using System.Threading.Tasks;

namespace ELog.Application.Dispensing.ReturnToVendor
{
    public interface IReturnToVendorAppService : IApplicationService
    {
        Task<HTTPResponseDto> GetMaterialDocumentNoAutoCompleteAsync(string input);

        Task<HTTPResponseDto> GetMaterialCodeByDocumentNo(string input);

        Task<HTTPResponseDto> GetSapBatchNumberByMaterialCodeAsync(string input);

        Task<HTTPResponseDto> UpdateReturnToVendorDtoAsync(ReturnToVendorDto input);

        Task<HTTPResponseDto> SaveReturnToVendorAsync(ReturnToVendorDto input);

        Task<HTTPResponseDto> PostReturnToVendorAsync(ReturnToVendorDto input);
    }
}