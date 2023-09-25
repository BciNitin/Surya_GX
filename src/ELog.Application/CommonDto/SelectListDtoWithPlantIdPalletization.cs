using Abp.Application.Services.Dto;

namespace ELog.Application.CommonDto
{
    public class SelectListDtoWithPlantIdPalletization : EntityDto<object>
    {
        public string Value { get; set; }
        public int? PlantId { get; set; }
        public bool? IsActive { get; set; }
        public bool IsSampling { get; set; }
        public bool IsReservationNo { get; set; }

        public string ProductBatchNo { get; set; }
        public string ContainerBarCode { get; set; }
        public string HUCode { get; set; }
        public string Name { get; set; }
    }
}