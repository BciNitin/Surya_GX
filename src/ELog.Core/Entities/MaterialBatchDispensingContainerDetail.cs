using ELog.Core.Authorization;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    public class MaterialBatchDispensingContainerDetail : PMMSFullAudit
    {
        [ForeignKey("MaterialBatchDispensingHeaderId")]
        public int MaterialBatchDispensingHeaderId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ContainerBarCode { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string SAPBatchNumber { get; set; }

        public float? Quantity { get; set; }

        public DateTime ContainerPickingTime { get; set; }

        public int? IsVerified { get; set; }
        public string verifiedBy { get; set; }
    }
}