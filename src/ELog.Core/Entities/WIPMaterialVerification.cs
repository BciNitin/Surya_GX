using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("WIPMaterialVerification")]

    public class WIPMaterialVerification : PMMSFullAudit
    {
        public int CubicleId { get; set; }
        public int? ProcessOrderId { get; set; }
        public int? ProductId { get; set; }
        public int CageBarcodeId { get; set; }
        public string CageBarcode { get; set; }
        public int NoOfCage { get; set; }
        public bool IsActive { get; set; }

    }
}
