using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.WIP.WIPSAP.MaterialReturnSAP.Dto
{
    [AutoMapTo(typeof(MaterialRteturnDetailsSAP))]
    public class MaterialRteturnDetailsSAPDto
    {
        public string MaterialDocumentNo { get; set; }
        public string MaterialDocumentYear { get; set; }
        public string ProductName { get; set; }
        public string ProductBatchNo { get; set; }
        public int? ProductId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string LineItemNo { get; set; }
        public string MaterialDescription { get; set; }
        public string ARNo { get; set; }
        public string SAPBatchNo { get; set; }
        public float? Qty { get; set; }
        public string UOM { get; set; }
    }
}
