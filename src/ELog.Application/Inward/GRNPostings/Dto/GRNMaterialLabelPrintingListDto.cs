using Abp.Application.Services.Dto;

using System.Collections.Generic;

namespace ELog.Application.Inward.GRNPostings.Dto
{
    public class GRNMaterialLabelPrintingListDto : EntityDto<int>
    {
        public string GRNNo { get; set; }
        public List<GRNMaterialLabelPrintingDto> GRNMaterialLabelPrintings { get; set; }
    }
}