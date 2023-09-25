using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("QC_Sampling")]
    public class QC_Sampling : Entity<int>
    {

        public int MaterialType { get; set; }
        public int InspectionLevel { get; set; }
        public string LotQuantityMin { get; set; }
        public string LotQuantityMax { get; set; }
        public int InspectionQuantity { get; set; }
    }
}
