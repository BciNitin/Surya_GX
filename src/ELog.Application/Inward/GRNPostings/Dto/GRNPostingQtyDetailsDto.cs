using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ELog.Core;
using ELog.Core.Entities;

using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Inward.VehicleInspections.Dto
{
    [AutoMapFrom(typeof(GRNQtyDetail))]
    public class GRNPostingQtyDetailsDto : EntityDto<int>
    {
        [Required]
        public int? GRNDetailId { get; set; }

        [Required]
        public float TotalQty { get; set; }

        [Required]
        public float NoOfContainer { get; set; }

        [Required]
        public float QtyPerContainer { get; set; }


        public string TotalQtyInDecimal { get; set; }

        public string QtyPerContainerInDecimal { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string DiscrepancyRemark { get; set; }
        public string IsDamaged { get; set; }
    }
}