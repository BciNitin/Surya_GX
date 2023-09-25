using Abp.AutoMapper;
using ELog.Core.Entities;
using System.Collections.Generic;

namespace ELog.Application.Dispensing.IssueToProductions.Dto
{
    [AutoMapTo(typeof(IssueToProduction))]
    public class IssueToProductionDetailDto
    {
        public string ProcessOrderNo { get; set; }
        public string MaterialCode { get; set; }
        public string LineItemNo { get; set; }
        public string MaterialDescription { get; set; }
        public string Product { get; set; }
        public string ProductBatch { get; set; }
        public string BatchNo { get; set; }
        public string ArNo { get; set; }
        public string SAPBatchNo { get; set; }
        public string DispensedQty { get; set; }
        public string UOM { get; set; }
        public string Storage_location { get; set; }
        public string MaterialIssueNoteNo { get; set; }
        public bool IsSelected { get; set; }
        public string MvtType { get; set; }

        public int? DispensingHeaderId { get; set; }
    }

    public class CreateIssueToProductionDto
    {
        public List<IssueToProductionDto> IssueToProductionDetails { get; set; }
    }
}