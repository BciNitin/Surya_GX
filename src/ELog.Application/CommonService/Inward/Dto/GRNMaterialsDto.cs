using Abp.Application.Services.Dto;

namespace ELog.Application.Inward.MaterialInspections.Dto
{
    public class GRNMaterialsDto : EntityDto<int>
    {
        public int? MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string InvoiceNo { get; set; }
        public int? InvoiceId { get; set; }
        public string ManufacturedBatchNo { get; set; }

        public float? QtyAsPerInvoice { get; set; }
    }
}