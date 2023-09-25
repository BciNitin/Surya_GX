using ELog.Core.Authorization;

using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("SAPPlantMaster")]
    public class SAPPlantMaster : PMMSFullAudit
    {
        public string PlantCode { get; set; }
        public string Description { get; set; }
        public string TaxRegNo { get; set; }
        public string License { get; set; }
        public string GS1Prefix { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}