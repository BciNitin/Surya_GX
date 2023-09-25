using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ELog.Application.WIP.CageLabelPrint.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.WIP.CageLabelPrint
{
    public interface ICageLabelPrintingService : IApplicationService
    {
        Task<CageLabelPrintingDto> CreateAsync(CageLabelPrintingDto input);

        Task<CageLabelPrintingDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<CageLabelPrintingListDto>> GetAllAsync(PagedCageLabelPrintResultRequestDto input);

        Task<CageLabelPrintingDto> UpdateAsync(CageLabelPrintingDto input);
        Task DeleteAsync(EntityDto<int> input);

        Task<PagedResultDto<CageLabelPrintingListDto>> GetListAsync();
        Task<List<string>> GetAllDispCode();
    }
}
