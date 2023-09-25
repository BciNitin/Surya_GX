using ELog.Core.Authorization;

using System;
using System.ComponentModel.DataAnnotations;

namespace ELog.Core.Entities
{
    public class SAPQualityControlDetail : PMMSFullAudit
    {
        [StringLength(PMMSConsts.Small)]
        public string ItemCode { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string InspectionlotNo { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string SAPBatchNo { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string BatchStockStatus { get; set; }

        public DateTime? RetestDate { get; set; }
        public DateTime? ReleasedOn { get; set; }
        public Decimal? ReleasedQty { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string MovementType { get; set; }
    }
}