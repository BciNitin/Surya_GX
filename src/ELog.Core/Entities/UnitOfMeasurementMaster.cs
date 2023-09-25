using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("UnitOfMeasurementMaster")]
    public class UnitOfMeasurementMaster : PMMSFullAuditWithApprovalStatus
    {
        [StringLength(PMMSConsts.Small)]
        public string UOMCode { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string Name { get; set; }

        public string Description { get; set; }

        [ForeignKey("UnitOfMeasurementTypeId")]
        public int? UnitOfMeasurementTypeId { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string ConversionUOM { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string UnitOfMeasurement { get; set; }

        public bool IsActive { get; set; }
        public int? TenantId { get; set; }

        [ForeignKey("UnitOfMeasurementId")]
        public ICollection<WeighingMachineMaster> WeighingMachineMasters { get; set; }

        [ForeignKey("UnitOfMeasurementId")]
        public ICollection<StandardWeightMaster> StandardWeightMasters { get; set; }

        [ForeignKey("UnitOfMeasurementId")]
        public ICollection<DispensingDetail> DispensingDetails { get; set; }

        [ForeignKey("UnitOfMeasurementId")]
        public ICollection<SampleDestruction> SampleDestructions { get; set; }
    }
}