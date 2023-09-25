using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

namespace ELog.Application.Masters.WeighingMachines.Dto
{
    [AutoMapFrom(typeof(WeighingMachineMaster))]
    public class WeighingMachineListDto : EntityDto<int>
    {
        public string WeighingMachineCode { get; set; }
        public int? SubPlantId { get; set; }
        public string UserEnteredPlantId { get; set; }

        public int? UnitOfMeasurementId { get; set; }
        public string UserEnteredUOM { get; set; }

        public string Make { get; set; }
        public string Modal { get; set; }

        public int? FrequencyTypeId { get; set; }
        public string RefrenceSOPNo { get; set; }
        public string FormatNo { get; set; }
        public string Version { get; set; }

        public string EccentricityInstruction { get; set; }
        public string LinearityInstruction { get; set; }
        public string RepeatabilityInstruction { get; set; }
        public string UncertaintyInstruction { get; set; }

        public float? MeanMinimumValue { get; set; }
        public float? MeanMaximumValue { get; set; }
        public bool IsActive { get; set; }
        public int ApprovalStatusId { get; set; }
        public string UserEnteredApprovalStatus { get; set; }
    }
}