using Abp.Application.Services.Dto;

namespace ELog.Application.WIP.MaterialReturn.Dto
{
    public class MaterialReturnListDto : EntityDto<int>
    {
        public string DocumentNo { get; set; }
        public int ProductId { get; set; }
        public string ProductNo { get; set; }
        public string BatchNo { get; set; }
        public int UOMId { get; set; }
        public string Uom { get; set; }
        public int ContainerId { get; set; }
        public string Container { get; set; }
        public int ScanBalanceId { get; set; }

        public string ScanBalanceNo { get; set; }

        public int Quantity { get; set; }
        public bool IsPrint { get; set; }

        public int PrintCount { get; set; }
        public bool IsActive { get; set; }
        public int? PrinterId { get; set; }
        public string MaterialReturnProcessLabelBarcode { get; set; }
        public string ProcessOrderId { get; set; }

    }
}
