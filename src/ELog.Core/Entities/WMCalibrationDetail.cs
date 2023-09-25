using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("WMCalibrationDetails")]
    public class WMCalibrationDetail : PMMSFullAudit
    {
        [ForeignKey("WMCalibrationHeaderId")]
        public int? WMCalibrationHeaderId { get; set; }

        [ForeignKey("CalibrationLevelId")]
        public int? CalibrationLevelId { get; set; }

        [ForeignKey("StandardWeightBoxId")]
        public int? StandardWeightBoxId { get; set; }

        public double CapturedWeight { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string Remark { get; set; }

        public string DoneBy { get; set; }
        public string CheckedBy { get; set; }

        public string SpriritLevelBubble { get; set; }

        [ForeignKey("CalibrationStatusId")]
        public int? CalibrationStatusId { get; set; }

        [ForeignKey("WMCalibrationDetailId")]
        public ICollection<WMCalibrationDetailWeight> WMCalibrationDetailWeights { get; set; }
    }
}