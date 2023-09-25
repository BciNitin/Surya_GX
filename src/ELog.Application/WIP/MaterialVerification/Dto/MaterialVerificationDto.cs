using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core;
using ELog.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.WIP.MaterialVerification.Dto
{
    [AutoMapFrom(typeof(WIPMaterialVerification))]
    public class MaterialVerificationDto : EntityDto<int>
    {
        public int? ProductID { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ProductCode { get; set; }

        public int? ProcessOrderId { get; set; }

        public int CubicleBarcodeId { get; set; }

        public int? CubicleId { get; set; }

        public string BatchNo { get; set; }

        public string UOM { get; set; }

        public string Quantity { get; set; }
        public string MaterialCode { get; set; }

        public string ARNo { get; set; }

        public string ProductName { get; set; }

        public string ProcessOrderNo { get; set; }

        public string CubicleBarcode { get; set; }
        public int CageBarcodeId { get; set; }
        public string CageBarcode { get; set; }
        public int NoOfCage { get; set; }
        public bool IsActive { get; set; }

    }
}
