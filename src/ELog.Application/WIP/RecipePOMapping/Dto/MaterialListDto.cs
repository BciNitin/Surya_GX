using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.WIP.RecipePOMapping.Dto
{
    [AutoMapFrom(typeof(ProcessOrderMaterialAfterRelease))]
    public class MaterialListDto : EntityDto<int>
    {
        public int? ProcessOrderId { get; set; }
        public string MaterialCode { get; set; }

        public string MaterialDescription { get; set; }

        public string ARNO { get; set; }

        public string LotNo { get; set; }

        public string SAPBatchNo { get; set; }

        public string CurrentStage { get; set; }

        public string NextStage { get; set; }

        public float Quantity { get; set; }
        public string UOM { get; set; }

    }
}
