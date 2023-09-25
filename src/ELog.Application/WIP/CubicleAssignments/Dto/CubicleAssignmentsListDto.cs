using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.WIP.CubicleAssignments.Dto
{
    [AutoMapFrom(typeof(CubicleAssignmentWIP))]
    public class CubicleAssignmentsListDto : EntityDto<int>
    {
        public int? ProductId { get; set; }
        public string ProductCode { get; set; }

        public int? ProcessOrderId { get; set; }

        public string ProcessOrderNo { get; set; }

        public int CubicleBarcodeId { get; set; }

        public string CubicleCode { get; set; }

        public int EquipmentBarcodeId { get; set; }

        public string EquipmentCode { get; set; }

        public string BatchNo { get; set; }

        public string LotNo { get; set; }
        public bool Status { get; set; }

    }
}
