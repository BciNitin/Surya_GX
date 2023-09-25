using Abp.Application.Services.Dto;

using ELog.Application.SelectLists.Dto;

using System;
using System.Collections.Generic;

namespace ELog.Application.Dispensing.Picking.Dto
{
    public class PickingValidationDto : EntityDto<int>
    {
        public string Value { get; set; }
        public int? PlantId { get; set; }
        public int? GroupStatusId { get; set; }
        public int? BatchPickingStatusId { get; set; }

        public List<SelectListDto> AssignedGroups { get; set; }
        public string SAPBatchNo { get; set; }
        public float? RequiredQty { get; set; }
        public DateTime? CreationTime { get; set; }
        public List<SelectListDto> SuggestedBins { get; set; }
    }
}