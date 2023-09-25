using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.Pallet.Dto
{
    [AutoMapTo(typeof(PalletMaster))]
    public class CreatePalletMasterDto
    {
        public int Id { get; set; }
        public string Pallet_Barcode { get; set; }
        public string Carton_barcode { get; set; }
        public string Description { get; set; }

        public string ProductBatchNo { get; set; }
        public int? TenantId { get; set; }

    }
}
