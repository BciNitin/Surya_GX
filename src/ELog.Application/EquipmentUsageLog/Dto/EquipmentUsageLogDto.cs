using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Application.Masters.InspectionChecklists.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.EquipmentUsageLog.Dto
{
    [AutoMapTo(typeof(ELog.Core.Entities.EquipmentUsageLog))]
    public class EquipmentUsageLogDto : EntityDto<int>
    {
        [Required(ErrorMessage = "Activity is required.")]
        public int ActivityId { get; set; }

        [Required(ErrorMessage = "Operator is required.")]
        public string OperatorName { get; set; }

        [Required(ErrorMessage = "EquipmentType is required.")]
        public string EquipmentType { get; set; }

        [Required(ErrorMessage = "EquipmentBracode is required.")]
        public int EquipmentBracodeId { get; set; }
        public int ProcessBarcodeId { get; set; }

        [Required(ErrorMessage = "EquipmentBracode is required.")]
        public string Remarks { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsActive { get; set; }

        public List<CheckpointDto> EquipmentUsageLogLists { get; set; }

        public string CubicalCode { get; set; }
        public string EquipmentCode { get; set; }

        public bool Status { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedTime { get; set; }
        public int? StatusId { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }

        public bool CanApproved { get; set; }
        public bool CanVerified { get; set; }
        public string ApprovedByName { get; set; }
        public string CreatorName { get; set; }

        public long? CreatorUserId { get; set; }

    }
}
