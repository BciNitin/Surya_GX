using Abp.AutoMapper;
using ELog.Core;
using ELog.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.WIP.MaterialVerification.Dto
{
    [AutoMapTo(typeof(WIPMaterialVerification))]
    public class CreateMaterialVerificationDto
    {
        public int? ProductID { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ProductCode { get; set; }

        public int? ProcessOrderId { get; set; }

        public int? CubicleBarcodeId { get; set; }

        public int? CubicleId { get; set; }

        public int? CageBarcodeId { get; set; }
        public string CageBarcode { get; set; }
        public int NoOfCage { get; set; }
        public bool IsActive { get; set; }
    }
}
