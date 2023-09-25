using Abp.AutoMapper;
using ELog.Core.Entities;

namespace ELog.Application.FinishedGoods.SAP_FG.Dto
{
    [AutoMapTo(typeof(OBDDetails))]
    public class OBDDetailDto
    {
        public string OBD { get; set; }
        public string LineItemNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductDesc { get; set; }
        public string ProductBatchNo { get; set; }
        public string ARNo { get; set; }
        public string SAPBatchNo { get; set; }
        public float? Qty { get; set; }
        public int? UOM { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
    }
}
