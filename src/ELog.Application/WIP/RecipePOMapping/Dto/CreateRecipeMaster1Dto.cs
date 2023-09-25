using Abp.AutoMapper;
using ELog.Core.Entities;
using System;
using System.Collections.Generic;

namespace ELog.Application.WIP.RecipePOMapping.Dto
{
    [AutoMapTo(typeof(RecipeTransactionHeader))]
    public class CreateRecipeMaster1Dto
    {
        public int ProductId { get; set; }

        public int ProcessOrderId { get; set; }
        public string DocumentVersion { get; set; }
        public string RecipeNo { get; set; }
        public int? ApprovedById { get; set; }
        public int? ApprovedLevelId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ApprovalRemarks { get; set; }
        public string ApprovalStatus { get; set; }
        public bool IsActive { get; set; }
        public List<CreayeRecipeMasterdetail1> RecipeTransactionDetails { get; set; }
    }
}
