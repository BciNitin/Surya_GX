using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.Dispensing.IssueToProductions
{
    [AutoMapFrom(typeof(IssueToProduction))]
    public class IssueToProductionDto : EntityDto<int>
    {
        public string ProcessOrderNo { get; set; }
        public string MaterialCode { get; set; }
        public string LineItemNo { get; set; }
        public string MaterialDescription { get; set; }
        public string Product { get; set; }
        public string ProductBatch { get; set; }
        public string ArNo { get; set; }
        public string SAPBatchNo { get; set; }
        public float? DispensedQty { get; set; }
        public string UOM { get; set; }
        public string MvtType { get; set; }

        public string Storage_location { get; set; }
        public string MaterialIssueNoteNo { get; set; }
        public bool IsSelected { get; set; }
        public int? TenantId { get; set; }

        public int? DispensingHeaderId { get; set; }
    }
}