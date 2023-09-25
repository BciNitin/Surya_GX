using Abp.Application.Services.Dto;

using ELog.Application.Masters.InspectionChecklists.Dto;

using System.Collections.Generic;

namespace ELog.Application.Inward.MaterialInspections.Dto
{
    public class AcceptRejectInspectionCheckpointDto : EntityDto<int>
    {
        public int MaterialRelationId { get; set; }
        public int TransactionStatusId { get; set; }

        public List<CheckpointDto> QualityCheckpoints { get; set; }
    }
}