using ELog.Core.Authorization;

using System;
using System.ComponentModel.DataAnnotations;

namespace ELog.Core.Entities
{
    public class SAPProcessOrderReceivedMaterial : PMMSFullAudit
    {
        [StringLength(PMMSConsts.Small)]
        public string Plant { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string PONo { get; set; }

        public DateTime? PODate { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string LineItemNo { get; set; }

        public decimal? OrderQty { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string UOM { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string ItemCode { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string ItemDescription { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string VendorName { get; set; }
    }
}