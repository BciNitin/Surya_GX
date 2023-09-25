using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("FormApprovalData")]
    public class FormApproval : Entity<int>
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }

    }
}



