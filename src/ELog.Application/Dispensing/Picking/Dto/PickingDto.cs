using Abp.Application.Services.Dto;
using ELog.Application.SelectLists.Dto;
using System.Collections.Generic;

namespace ELog.Application.Dispensing.Picking.Dto
{
    public class PickingDto : EntityDto<int>
    {
        public string CubicleCode { get; set; }
        public int? CubicleId { get; set; }
        public string GroupId { get; set; }
        public List<SelectListDto> SuggestedBins { get; set; }

        public string MaterialCode { get; set; }
        public string MaterialContainerBarCode { get; set; }
        public int MaterialContainerId { get; set; }
        public float? RequiredQty { get; set; }
        public string SAPBatchNo { get; set; }
        public List<string> SAPBatchNumbers { get; set; }

        public string CommaSeparatedSuggestedBins { get; set; }
        public string LocationBarCode { get; set; }
        public int? LocationId { get; set; }
        public int? CubicleAssignmentHeaderId { get; set; }
        public int ContainerCount { get; set; }
        public float? Quantity { get; set; }
        public bool IsCompletePickingAllowed { get; set; }
    }

    public class PickingCompleteCartesianDto
    {
        public string CubicleCode { get; set; }
        public string MaterialCode { get; set; }
        public string SAPBatchNumber { get; set; }
    }
}