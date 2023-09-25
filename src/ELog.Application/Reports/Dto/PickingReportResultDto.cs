using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

using System;

namespace ELog.Application.Reports.Dto
{
    [AutoMapFrom(typeof(MaterialBatchDispensingHeader))]
    public class pickingReportResultDto : EntityDto<int>
    {
        public DateTime PickingTime { get; set; }
        public string PlantCode { get; set; }
        public string CubicleCode { get; set; }
        public string GroupCode { get; set; }
        public string ProductCode { get; set; }
        public string MaterialCode { get; set; }
        public string BatchNo { get; set; }
        public string SapBatchNo { get; set; }
        public string Status { get; set; }
        public string LocationCode { get; set; }

        public float? BalQty { get; set; }
        public float? Qty { get; set; }
        public string ContainerBarcode { get; set; }
        public string PickedBy { get; set; }
        public int? StatusId { get; set; }
        public int? SubPlantId { get; set; }
        public int? CubicleId { get; set; }
        public int? MaterialId { get; set; }
        public int? ContainerId { get; set; }
        public int? ProcessOrderId { get; set; }
        public int? InspectionLotId { get; set; }
        public int? LocationId { get; set; }
        public bool? IsSampling { get; set; }
        public long? NoOfContainers { get; set; }
        public int? MaterialTransferTypeId { get; set; }
    }
}