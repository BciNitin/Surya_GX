using Abp.Application.Services.Dto;
using System;

namespace ELog.Application.EquipmentUsageLog.Dto
{
    public class EquipmentUsageLogListDto : EntityDto<int>
    {
        public int? ActivityId { get; set; }

        public string ActivityName { get; set; }

        //public int? OperatorId { get; set; }
        public string OperatorName { get; set; }
        public string EquipmentType { get; set; }
        public int? equipmentBracodeId { get; set; }

        public string equipmentBracodeName { get; set; }
        public int? processBarcodeId { get; set; }

        public string processBarcodeName { get; set; }
        public DateTime? StartTime { get; set; }

        public DateTime? EndtTime { get; set; }

        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedTime { get; set; }
        public int? StatusId { get; set; }
    }
}
