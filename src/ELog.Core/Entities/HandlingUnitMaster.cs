
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("HandlingUnitMaster")]
    public class HandlingUnitMaster : PMMSFullAuditWithApprovalStatus
    {
        [ForeignKey("PlantId")]
        public int PlantId { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string HUCode { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string Name { get; set; }

        [ForeignKey("HandlingUnitTypeId")]
        public int? HandlingUnitTypeId { get; set; }

        public bool IsActive { get; set; }
        public string Description { get; set; }
        public int? TenantId { get; set; }

        [ForeignKey("PalletId")]
        public ICollection<Palletization> Palletizations { get; set; }

        [ForeignKey("PalletId")]
        public ICollection<PutAwayBinToBinTransfer> PutAwayBinToBinTransfers { get; set; }
    }
}