using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

using System;

namespace ELog.Core.SQLDtoEntities
{
    [AutoMapFrom(typeof(MaterialBatchDispensingHeader))]
    public class PickingReportDto : EntityDto<int>
    {
        public int SubPlantId { get; set; }
        public int? ProcessOrderId { get; set; }

        public int? InspectionLotId { get; set; }

        public DateTime PickingTime { get; set; }
        public string PlantCode { get; set; }
        public string CubicleCode { get; set; }
        public string GroupId { get; set; }
        public string ProductCode { get; set; }
        public string MaterialCode { get; set; }
        public string ContainerBarcode { get; set; }
        public string BatchNo { get; set; }
        public string SapBatchNo { get; set; }
        public string LocationCode { get; set; }
        public float? Quantity { get; set; }
        public string PickedBy { get; set; }
        public bool? IsSampling { get; set; }
        public int NoOfContainers { get; set; }
        public string InspectionLotNo { get; set; }
        public string ProcessOrderNo { get; set; }
    }
}