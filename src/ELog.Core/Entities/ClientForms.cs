using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elog.Core.Entities
{
    [Table("Elog_ClientForms")]
    public class ClientForm : Entity<int>
    {
        public int ClientId { get; set; }
        public string FormName { get; set; }
        public DateTime FormStartDate { get; set; }
        public DateTime FormEndDate { get; set; }
        public string FormJson { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public int FormStatus { get; set; }
        public string UpdatedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string CheckedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ApproveDateTime { get; set; }

        public string Permissions { get; set; }
        public string MenuId { get; set; }
    }
}
