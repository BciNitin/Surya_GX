using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.Collections.Generic;

namespace ELog.Application.WIP.Consumption.Dto
{
    [AutoMapTo(typeof(ELog.Core.Entities.Consumption))]
    public class ConsumptionDto : EntityDto<int>
    {
        public int? CubicleId { get; set; }
        public int? ProductId { get; set; }
        public string ProductCode { get; set; }
        public int? ProcessOrderId { get; set; }
        public int? EquipmentId { get; set; }

        public int? NoOfContainer { get; set; }


        public List<ConsumptionDetailDto> ConsumptionDetails { get; set; }
    }
}
