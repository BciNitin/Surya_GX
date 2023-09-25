using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace ELog.Application.WIP.RecipePOMapping.Dto
{

    public class CreateRecipeDetailsDto : EntityDto<int>
    {
        public int ProductId { get; set; }

        public int ProcessOrderId { get; set; }
        public string ProductCode { get; set; }

        public string ProductName { get; set; }
        public string ProcessOrderNo { get; set; }
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
