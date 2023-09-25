using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.Pallet.Dto
{
    [AutoMapFrom(typeof(PalletMaster))]
    public class PalletMasterListDto : EntityDto<int>
    {
        public string Pallet_Barcode { get; set; }
        public string Carton_barcode { get; set; }

        public string Description { get; set; }
        //public string ProductBatchNo { get; set; }
        public int? TenantId { get; set; }
        public int PalletBarcodeId { get; set; }
        public int CartonBarcodeId { get; set; }
        public string ProductBatchNo { get; set; }


    }
}
