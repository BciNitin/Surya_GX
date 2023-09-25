
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("LocationMaster")]
    public class LocationMaster : PMMSFullAuditWithApprovalStatus
    {
        [StringLength(PMMSConsts.Small)]
        public string LocationCode { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string StorageLocationType { get; set; }

        [ForeignKey("PlantMaster")]
        public int PlantId { get; set; }

        [ForeignKey("DepartmentMaster")]
        public int DepartmentId { get; set; }

        [ForeignKey("AreaMaster")]
        public int AreaId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string Zone { get; set; }

        public decimal? LocationTemperature { get; set; }

        public decimal? LocationTemperatureUL { get; set; }

        public int? TemperatureUnit { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string SLOCType { get; set; }

        public int? LevelId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? TenantId { get; set; }

        [ForeignKey("LocationId")]
        public ICollection<PutAwayBinToBinTransfer> PutAwayBinToBinTransfers { get; set; }
    }
}