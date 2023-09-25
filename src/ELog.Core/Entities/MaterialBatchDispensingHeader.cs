using ELog.Core.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    public class MaterialBatchDispensingHeader : PMMSFullAudit
    {
        [StringLength(PMMSConsts.Medium)]
        public string CubicleCode { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string GroupCode { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string MaterialCode { get; set; }

        public int StatusId { get; set; }
        public DateTime PickingTime { get; set; }
        public int BatchPickingStatusId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string SAPBatchNumber { get; set; }

        public int MaterialBatchDispensingHeaderType { get; set; }
        public bool IsSampling { get; set; }
        public int? TenantId { get; set; }



        [ForeignKey("MaterialBatchDispensingHeaderId")]
        public ICollection<MaterialBatchDispensingContainerDetail> MaterialBatchDispensingContainerDetails { get; set; }
    }
}