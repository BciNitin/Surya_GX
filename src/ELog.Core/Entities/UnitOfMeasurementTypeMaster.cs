using ELog.Core.Authorization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("UnitOfMeasurementTypeMaster")]
    public class UnitOfMeasurementTypeMaster : PMMSFullAudit
    {
        [Required]
        [StringLength(PMMSConsts.Medium)]
        public string UnitOfMeasurementTypeName { get; set; }

        [ForeignKey("UnitOfMeasurementTypeId")]
        public ICollection<UnitOfMeasurementMaster> UnitOfMeasurementMasters { get; set; }
    }
}