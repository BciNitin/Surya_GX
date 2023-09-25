using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Dispensing.CubicleAssignments.Dto
{
    [AutoMapFrom(typeof(CubicleAssignmentDetail))]
    public class CubicleAssignmentDetailsDto : EntityDto<int>
    {
        public int? CubicleAssignmentHeaderId { get; set; }

        public int? CubicleId { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string CubicleBarcode { get; set; }

        public int? ProcessOrderId { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string ProcessOrderNo { get; set; }

        public int? InspectionLotId { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string InspectionLotNo { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string ProductCode { get; set; }

        public string LineItemNo { get; set; }

        public int? ProcessOrderMaterialId { get; set; }

        public string MaterialCode { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string MaterialDescription { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string BatchNo { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string SAPBatchNumber { get; set; }

        public float? Qty { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string UnitOfMeasurement { get; set; }

        public int? UnitOfMeasurementId { get; set; }

        public string ExpiryDate { get; set; }
        public string RetestDate { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string ARNo { get; set; }
        public bool IsAssigned { get; set; }
        public int? TenantId { get; set; }
        public int? StatusId { get; set; }
        public bool IsReservationNo { get; set; }
    }
}