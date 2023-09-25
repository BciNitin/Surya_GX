using ELog.Core.Authorization;

using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("WeighingMachineTestConfigurations")]
    public class WeighingMachineTestConfiguration : PMMSFullAudit
    {
        [ForeignKey("WeighingMachineId")]
        public int? WeighingMachineId { get; set; }

        [ForeignKey("FrequencyTypeId")]
        public int? FrequencyTypeId { get; set; }

        public bool? IsEccentricityTestRequired { get; set; }
        public bool? IsLinearityTestRequired { get; set; }
        public bool? IsRepeatabilityTestRequired { get; set; }
        public bool? IsUncertainityTestRequired { get; set; }
    }
}