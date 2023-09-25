using Abp.Application.Services.Dto;

namespace ELog.Application.Dispensing.ReturnToVendor.Dto
{
    public class ReturnToVendorDto : EntityDto<int>
    {
        public string MaterialDocumentId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialContainerBarCode { get; set; }
        public string SAPBatchNumber { get; set; }
        public string ArNo { get; set; }
        public int? StatusId { get; set; }
        public int ContainerCount { get; set; }
        public float? Quantity { get; set; }
        public float? ScanQty { get; set; }
        public string UOM { get; set; }
        public float? ConvertedQty { get; set; }
        public string BaseUom { get; set; }
    }

    public class ReturnToVendorUnitOfMeasurementDto
    {
        public int Id { get; set; }
        public string UnitOfMeasurement { get; set; }
        public string UomType { get; set; }
    }
}