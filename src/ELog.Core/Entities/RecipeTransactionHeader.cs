using ELog.Core.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELog.Core.Entities
{
    [Table("RecipeTransactionHeader")]
    public class RecipeTransactionHeader : PMMSFullAudit
    {
        public int ProductId { get; set; }
        public string DocumentVersion { get; set; }
        public string RecipeNo { get; set; }

        [ForeignKey("Users")]
        public int? ApprovedById { get; set; }

        [ForeignKey("ApprovalLevelMaster")]
        public int? ApprovedLevelId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ApprovalRemarks { get; set; }
        public string ApprovalStatus { get; set; }

        [ForeignKey("Users")]
        public int? RejectedById { get; set; }
        public DateTime? RejectedDate { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<RecipeTransactionDetails> RecipeTransactionDetails { get; set; }
    }
}
