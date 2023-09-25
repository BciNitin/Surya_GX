using Abp.AutoMapper;
using ELog.Core;
using ELog.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.WIP.CubicleAssignments.Dto
{
    [AutoMapTo(typeof(CubicleAssignmentWIP))]
    public class CreateCubicleAssignmentsDto
    {
        public int? ProductId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ProductCode { get; set; }

        public int? ProcessOrderId { get; set; }

        public int CubicleBarcodeId { get; set; }

        public int EquipmentBarcodeId { get; set; }


    }
}
