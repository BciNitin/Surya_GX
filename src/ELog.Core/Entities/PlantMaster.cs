using ELog.Core.Authorization.Users;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("PlantMaster")]
    public class PlantMaster : PMMSFullAuditWithApprovalStatus
    {
        public int? TenantId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string PlantName { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string PlantId { get; set; }

        public int? MasterPlantId { get; set; }
        public int? PlantTypeId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string? TaxRegistrationNo { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string? License { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string? GS1Prefix { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string? Address1 { get; set; }

        [StringLength(PMMSConsts.Large)]
        public string? Address2 { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string? PostalCode { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string? City { get; set; }

        [ForeignKey("StateId")]
        public int? StateId { get; set; }

        [ForeignKey("CountryId")]
        public int? CountryId { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string? Email { get; set; }

        [StringLength(PMMSConsts.Small)]
        public string? PhoneNumber { get; set; }

        [StringLength(PMMSConsts.Medium)]
        public string? Website { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }

        [ForeignKey("PlantId")]
        public ICollection<User> Users { get; set; }

        [ForeignKey("PlantId")]
        public ICollection<GateMaster> GateMasters { get; set; }

        [ForeignKey("PlantId")]
        public ICollection<LocationMaster> LocationMasters { get; set; }

        [ForeignKey("MasterPlantId")]
        public ICollection<PlantMaster> MasterPlants { get; set; }

        [ForeignKey("PlantId")]
        public ICollection<CubicleMaster> CubicleMasters { get; set; }

        [ForeignKey("PlantId")]
        public ICollection<EquipmentMaster> EquipmentMasters { get; set; }

        [ForeignKey("PlantId")]
        public ICollection<HandlingUnitMaster> HandlingUnitMasters { get; set; }

        [ForeignKey("SubPlantId")]
        public ICollection<StandardWeightBoxMaster> StandardWeightBoxMasters { get; set; }

        [ForeignKey("SubPlantId")]
        public ICollection<StandardWeightMaster> StandardWeightMasters { get; set; }

        [ForeignKey("SubPlantId")]
        public ICollection<DepartmentMaster> DepartmentMasters { get; set; }

        [ForeignKey("SubPlantId")]
        public ICollection<AreaMaster> AreaMasters { get; set; }

        [ForeignKey("SubPlantId")]
        public ICollection<WeighingMachineMaster> WeighingMachineMasters { get; set; }

        [ForeignKey("PlantId")]
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }

        [ForeignKey("SubPlantId")]
        public virtual ICollection<ChecklistTypeMaster> ChecklistTypeMasters { get; set; }

        [ForeignKey("PlantId")]
        public ICollection<UserPlants> UserPlants { get; set; }
    }
}