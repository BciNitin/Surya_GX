using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.WIP.InProcessLabel.Dto
{
    [AutoMapFrom(typeof(InProcessLabelDetails))]
    public class ProcessLabelDto : EntityDto<int>
    {
        [Required(ErrorMessage = "Cubicle is required.")]
        public int CubicleId { get; set; }

        public int? ProcessOrderId { get; set; }

        public int? ProductId { get; set; }

        public string ProcessLabelBarcode { get; set; }
        public string ProcessOrderNo { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string BatchNo { get; set; }

        public string CurrentStage { get; set; }

        public string NextStage { get; set; }

        public string ContainerBarcode { get; set; }

        public int? ScanBalanceId { get; set; }

        public string ScanBalance { get; set; }
        public float? GrossWeight { get; set; }
        public float? NetWeight { get; set; }
        public float? TareWeight { get; set; }

        public string NoOfContainer { get; set; }

        public bool IsPrint { get; set; }

        public int PrintCount { get; set; }
        public bool IsActive { get; set; }
        public int? PrinterId { get; set; }

        //public DateTime ExperyDate { get; set; }
        //public DateTime MfgDate { get; set; }
    }
}
