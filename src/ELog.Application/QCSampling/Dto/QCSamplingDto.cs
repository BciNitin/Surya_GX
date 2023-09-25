using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.QCSampling.Dto
{
    [AutoMapFrom(typeof(QC_Sampling))]
    public class QCSamplingDto : EntityDto<int>
    {
        public int MaterialType { get; set; }
        public int InspectionLevel { get; set; }
        public string LotQuantityMin { get; set; }
        public string LotQuantityMax { get; set; }
        public int InspectionQuantity { get; set; }
    }
}
