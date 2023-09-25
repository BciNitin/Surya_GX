using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("DeviceMaster")]
    public class DeviceMaster : PMMSFullAuditWithApprovalStatus
    {
        [ForeignKey("SubPlantId")]
        public int SubPlantId { get; set; }

        [MaxLength(PMMSConsts.Small)]
        public string DeviceId { get; set; }

        [ForeignKey("DeviceTypeId")]
        public int? DeviceTypeId { get; set; }

        [MaxLength(PMMSConsts.Medium)]
        public string Make { get; set; }

        [MaxLength(PMMSConsts.Medium)]
        public string Model { get; set; }

        [MaxLength(PMMSConsts.Medium)]
        public string SerialNo { get; set; }

        [MaxLength(PMMSConsts.Small)]
        public string IpAddress { get; set; }

        public int? Port { get; set; }

        [ForeignKey("DepartmentId")]
        public int? DepartmentId { get; set; }

        [ForeignKey("AreaId")]
        public int? AreaId { get; set; }

        [ForeignKey("CubicleId")]
        public int? CubicleId { get; set; }

        [ForeignKey("ModeId")]
        public int? ModeId { get; set; }

        public bool IsActive { get; set; }

        public int? TenantId { get; set; }
        [ForeignKey("PrinterId")]
        public ICollection<GRNMaterialLabelPrintingDetail> GRNMaterialLabelPrintingDetails { get; set; }

        [ForeignKey("DeviceId")]
        public ICollection<DispensingPrintDetail> DispensingPrintDetails { get; set; }
    }
}