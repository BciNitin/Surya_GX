using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("CubicleAssignmentWIP")]
    public class CubicleAssignmentWIP : PMMSFullAudit
    {

        public int? ProductId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ProductCode { get; set; }

        [ForeignKey("ProcessOrderId")]
        public int? ProcessOrderId { get; set; }

        [ForeignKey("CubicleMaster")]
        public int CubicleBarcodeId { get; set; }

        [ForeignKey("EquipmentMaster")]
        public int EquipmentBarcodeId { get; set; }

        public bool Status { get; set; }
    }
}