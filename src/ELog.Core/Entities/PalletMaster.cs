using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("PalletMaster")]
    public class PalletMaster : PMMSFullAudit
    {

        public string Pallet_Barcode { get; set; }
        public string Carton_barcode { get; set; }

        public string Description { get; set; }
        public string ProductBatchNo { get; set; }
        public int? TenantId { get; set; }
        public int PalletBarcodeId { get; set; }
        public int CartonBarcodeId { get; set; }
    }
}