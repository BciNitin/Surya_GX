using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("FrequencyTypeMaster")]
    public class FrequencyTypeMaster : PMMSFullAudit
    {
        [StringLength(PMMSConsts.Small)]
        public string FrequencyName { get; set; }

        [ForeignKey("CalibrationFrequencyId")]
        public ICollection<WMCalibrationHeader> WMCalibrationHeaders { get; set; }

        [ForeignKey("FrequencyTypeId")]
        public ICollection<CalibrationFrequencyMaster> CalibrationFrequencyMasters { get; set; }

        [ForeignKey("FrequencyTypeId")]
        public ICollection<WeighingMachineTestConfiguration> WeighingMachineTestConfigurations { get; set; }
    }
}