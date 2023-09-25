using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.WIP.PostToSAP.Dto
{
    [AutoMapFrom(typeof(PostWIPDataToSAP))]
    public class PostToSAPDto : EntityDto<int>
    {
        [Required(ErrorMessage = "ProductID is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "ProcessOrderId is required.")]
        public int ProcessOrderId { get; set; }

        public string ProcessOrderNo { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }


        public string BatchNo { get; set; }

        public bool IsActive { get; set; }
        public bool IsSent { get; set; }
    }
}
