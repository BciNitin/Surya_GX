using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.SelectLists.Dto;
using ELog.Application.WIP.WIPPrintPacking.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.WIP.WIPPrintPacking
{
    public interface IPrintPackingService : IApplicationService
    {
        Task<PrintPackingDto> CreateAsync(CreatePrintPackingDto input);
        Task<PrintPackingDto> GetAsync(EntityDto<int> input);
        Task<PrintPackingDto> UpdateAsync(PrintPackingDto input);
        Task<PagedResultDto<PrintPackingListDto>> GetAllAsync(PagedPrintPackingResultRequestDto input);
        Task<PrintPackingDto> GetListAsync(EntityDto<int> input);
        Task DeleteAsync(EntityDto<int> input);
        Task<PrintPackingDto> GetProductNamebyProductcodeAsync(EntityDto<string> input);
        Task<List<SelectListDto>> GetAllContainerBarcode();
    }
}
