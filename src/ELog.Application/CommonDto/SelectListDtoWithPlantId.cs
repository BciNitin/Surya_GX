using Abp.Application.Services.Dto;

namespace ELog.Application.CommonDto
{
    public class SelectListDtoWithPlantId : EntityDto<object>
    {
        public string Value { get; set; }
        public int? PlantId { get; set; }
        public bool? IsActive { get; set; }
        public bool IsSampling { get; set; }
        public bool IsReservationNo { get; set; }

        public int? LeastCountDigitAfterDecimal { get; set; }

    }
}