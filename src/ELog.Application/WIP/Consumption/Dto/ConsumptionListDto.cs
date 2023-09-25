using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace ELog.Application.WIP.Consumption.Dto
{
    [AutoMapTo(typeof(ELog.Core.Entities.Consumption))]
    public class ConsumptionListDto : EntityDto<int>
    {
        public int? CubicleId { get; set; }
        public string ProductId { get; set; }
        public string CubicalCode { get; set; }
        // public int? ProductId { get; set; }
        public string ProductCode { get; set; }
        public int? ProcessOrderId { get; set; }
        public string ProcessOrderNo { get; set; }
        public int? EquipmentId { get; set; }
        public int? processBarcodeId { get; set; }
        public int? equipmentBracodeId { get; set; }

        public string EquipmentNo { get; set; }
        public int? NoOfContainer { get; set; }
    }
}
