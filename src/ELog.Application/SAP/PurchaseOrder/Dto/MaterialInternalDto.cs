using Abp.Application.Services.Dto;

namespace ELog.Application.SAP.PurchaseOrder.Dto
{
    public class MaterialInternalDto : EntityDto<int>
    {
        public string Number { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public float Quantity { get; set; }
        public float? InvoiceQuantity { get; set; }
        public float? PickedInvoiceQuantity { get; set; }
        public float? BalanceQuantity { get; set; }
        public string UOM { get; set; }
        public string ManufacturerName { get; set; }
        public string ManufacturerCode { get; set; }
    }
}