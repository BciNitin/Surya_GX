using Abp.Application.Services.Dto;

using System;
namespace ELog.Application.CommonService.MaterialStatus.Dto
{
    public class MaterialStatusDto : EntityDto<int>
    {
        public string MaterialCode { get; set; }
        public string MaterialDescription { get; set; }
        public string SapBatchNo { get; set; }
        public DateTime? MfgDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int DaysLeftForExpiry { get; set; }
        public DateTime? RetestDate { get; set; }
        public int DaysLeftForRetest { get; set; }
        public int PlantId { get; set; }
        public int GrnDetailId { get; set; }
    }
}
