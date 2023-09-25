using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core;
using ELog.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.WIP.CubicleAssignments.Dto
{
    [AutoMapFrom(typeof(CubicleAssignmentWIP))]
    public class CubicleAssignmentsDto : EntityDto<int>
    {
        public int? ProductId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ProductCode { get; set; }

        public int? ProcessOrderId { get; set; }

        public int CubicleBarcodeId { get; set; }

        public int EquipmentBarcodeId { get; set; }

        public string BatchNo { get; set; }

        public string LotNo { get; set; }

        public string ProductName { get; set; }

        public string ProcessOrderNo { get; set; }

        public string CubicleBarcode { get; set; }
        public string EquipmentBarcode { get; set; }
        public string EquipmentType { get; set; }
        public bool Status { get; set; }

    }
}
