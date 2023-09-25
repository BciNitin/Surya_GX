using Abp.AutoMapper;
using ELog.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.WIP.PostToSAP.Dto
{
    [AutoMapTo(typeof(PostWIPDataToSAP))]
    public class CreatePostToSAPDto
    {
        [Required(ErrorMessage = "ProductID is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "ProcessOrderId is required.")]
        public int ProcessOrderId { get; set; }

        public bool IsActive { get; set; }
        public bool IsSent { get; set; }
    }
}
