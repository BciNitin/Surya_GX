using Abp.Application.Services.Dto;
using ELog.Application.SelectLists.Dto;
using System.Collections.Generic;

namespace ELog.Application.Dispensing.Destruction.Dto
{
    public class StagingOutDto : EntityDto<int>
    {
        public string CubicleCode { get; set; }
        public int? CubicleId { get; set; }
        public int? CubicleAssignmentHeaderId { get; set; }

        public string GroupId { get; set; }

        public int? InspectionLotId { get; set; }

        public string MaterialCode { get; set; }
        public string SAPBatchNo { get; set; }
        public string MaterialContainerBarcode { get; set; }

        public float BalanceQuantity { get; set; }
        public int ContainerCount { get; set; }
        public float Quantity { get; set; }
    }

    public class CubicleStageOutInternalDto
    {
        public int CubicleId { get; set; }
        public string CubicleCode { get; set; }
        public int PlantId { get; set; }
        public List<SelectListDto> lstGroupOrInspectionSelectList { get; set; }
    }
}