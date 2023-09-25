using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("CalibrationStatusMaster")]
    public class CalibrationStatusMaster : PMMSFullAudit
    {
        [StringLength(PMMSConsts.Small)]
        public string StatusName { get; set; }

        [ForeignKey("CalibrationStatusId")]
        public ICollection<WMCalibrationHeader> WMCalibrationHeaders { get; set; }

        [ForeignKey("CalibrationStatusId")]
        public ICollection<WMCalibrationDetail> WMCalibrationDetails { get; set; }
    }
}