using Abp.Application.Services;
using Abp.Application.Services.Dto;

using ELog.Application.CommonDto;
using ELog.Application.Inward.GRNPostings.Dto;
using ELog.Application.Inward.VehicleInspections.Dto;
using ELog.Application.SelectLists.Dto;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELog.Application.Inward.GRNs
{
    public interface IGRNPostingAppService : IApplicationService
    {
        Task<GRNPostingDto> GetAsync(EntityDto<int> input);

        Task<PagedResultDto<GRNPostingListDto>> GetAllAsync(PagedGRNPostingResultRequestDto input);

        Task<HTTPResponseDto> CreateAsync(CreateGRNPostingDto input);

        Task<GRNMaterialLabelPrintingListDto> GetGRNDetailWithAllMaterialLabelPrinting(int input);

        Task<List<SelectListDto>> GetAllGRNHeaders(string input);

        Task<GRNMaterialLabelPrintingDto> PrintMaterialLabels(GRNMaterialLabelPrintingDto input);

        Task<GRNMaterialLabelPrintingDto> PrintMaterialLabelRange(GRNMaterialLabelPrintingDto input);
    }
}