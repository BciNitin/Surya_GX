using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.WIP.PostToSAP.Dto
{
    [AutoMapFrom(typeof(PostWIPDataToSAP))]
    public class PostToSAPListDto : EntityDto<int>
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }

        public string ProductName { get; set; }
        public int ProcessOrderId { get; set; }
        public string ProcessOrderNo { get; set; }

        public string BatchNo { get; set; }
        public string CurrentStage { get; set; }

        public int ContainerBarcodeCount { get; set; }

        public float NetWeight { get; set; }

        public bool IsActive { get; set; }
        public bool IsSent { get; set; }
    }
}
