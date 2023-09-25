using Abp.Application.Services.Dto;

namespace ELog.Application.Masters.WeighingMachines.Dto
{
    public class WeighingMachineTestconfigurationDto : EntityDto<int>
    {
        public int? WeighingMachineId { get; set; }
        public int? FrequencyTypeId { get; set; }

        public bool? IsEccentricityTestRequired { get; set; }
        public bool? IsLinearityTestRequired { get; set; }
        public bool? IsRepeatabilityTestRequired { get; set; }
        public bool? IsUncertainityTestRequired { get; set; }
    }
}