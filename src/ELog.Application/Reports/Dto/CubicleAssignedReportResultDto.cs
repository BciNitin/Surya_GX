using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core.Entities;

using System;

namespace ELog.Application.Reports.Dto
{
    [AutoMapFrom(typeof(CubicleAssignmentHeader))]
    public class CubicleAssignedReportResultDto : EntityDto<int>
    {
        public DateTime CubicleAssignmentDate { get; set; }
        public string PlantCode { get; set; }
        public string CubicleCode { get; set; }
        public string GroupCode { get; set; }
        public string ProductCode { get; set; }
        public string BatchNo { get; set; }
        public string MaterialCode { get; set; }
        public string SapBatchNo { get; set; }
        public float? BalQty { get; set; }
        public float? Qty { get; set; }
        public string CubicleStatus { get; set; }
        public int? SubPlantId { get; set; }
        public int? CubicleId { get; set; }
        public int? GroupId { get; set; }
        public int? MaterialId { get; set; }
        public int? GrnDetailId { get; set; }
        public bool IsCubicleAssigned { get; set; }
        public int? GroupStatusId { get; set; }
        public string GroupStatus { get; set; }
        public bool? IsSampling { get; set; }
        public string AssignmentBy { get; set; }
    }
}