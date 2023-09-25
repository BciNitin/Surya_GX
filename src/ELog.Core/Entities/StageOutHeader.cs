using ELog.Core.Authorization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("StageOutHeaders")]
    public class StageOutHeader : PMMSFullAudit
    {
        [ForeignKey("CubicleId")]
        public int CubicleId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string GroupId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public int? InspectionLotId { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string MaterialCode { get; set; }

        [ForeignKey("StatusId")]
        public int StatusId { get; set; }

        public bool IsSampling { get; set; }

        [ForeignKey("StageOutHeaderId")]
        public ICollection<StageOutDetail> StageOutDetails { get; set; }
    }
}