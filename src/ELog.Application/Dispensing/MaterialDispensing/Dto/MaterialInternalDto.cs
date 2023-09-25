using Abp.Application.Services.Dto;

namespace ELog.Application.Dispensing.MaterialDispensing.Dto
{
    public class MaterialInternalDto : EntityDto<int>
    {
        public int UomId { get; set; }
        public float Denominator { get; set; }
        public float Numerator { get; set; }
        public string ConversionUOMName { get; set; }
        public int? DoneBy { get; set; }
        public int? CheckedById { get; set; }
    }
}