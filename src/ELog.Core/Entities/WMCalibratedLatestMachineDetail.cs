using ELog.Core.Authorization;

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("WMCalibratedLatestMachineDetails")]
    public class WMCalibratedLatestMachineDetail : PMMSFullAudit
    {
        [ForeignKey("WMCalibrationHeaderId")]
        public int? WMCalibrationHeaderId { get; set; }

        [ForeignKey("WeighingMachineId")]
        public int? WeighingMachineId { get; set; }

        public DateTime LastCalibrationTestDate { get; set; }
    }
}