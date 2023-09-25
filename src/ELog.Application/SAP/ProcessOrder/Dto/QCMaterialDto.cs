using ELog.Core;

using System;
using System.ComponentModel.DataAnnotations;

namespace ELog.Application.SAP.ProcessOrder.Dto
{
    public class QCMaterialDto
    {
        [StringLength(PMMSConsts.Medium)]
        [Required]
        public string ProcessOrderNo { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string ItemNo { get; set; }

        public int? GRNHeaderId { get; set; }

        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string ItemCode { get; set; }

        public string ItemDescription { get; set; }

        public float OrderQuantity { get; set; }
        public string UnitOfMeasurement { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string BatchNo { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string SAPBatchNo { get; set; }

        public DateTime ExpiryDate { get; set; }

        public DateTime RetestDate { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string ARNo { get; set; }
    }
}