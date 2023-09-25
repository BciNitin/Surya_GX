using Abp.Application.Services.Dto;

namespace ELog.Application.Dispensing.MaterialSampleDispensing.Dto
{
    public class MaterialSampleDispensingInternalDto : EntityDto<int>
    {
        public string ConversionUOMName { get; set; }
        public float? Denominator { get; set; }
        public float? Numerator { get; set; }
        public int? UomId { get; set; }
        public int? UnitOfMeasurementTypeId { get; set; }
        public string UOMType { get; set; }
        public bool IsPackUOM { get; set; }
    }
}