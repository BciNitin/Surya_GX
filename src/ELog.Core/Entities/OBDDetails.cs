using ELog.Core.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("OBDDetails")]
    public class OBDDetails : PMMSFullAudit
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
