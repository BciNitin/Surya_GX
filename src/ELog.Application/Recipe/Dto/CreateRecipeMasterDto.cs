using Abp.AutoMapper;
using ELog.Core.Entities;
using System;
using System.Collections.Generic;

namespace ELog.Application.Recipe.Dto
{
    [AutoMapTo(typeof(RecipeTransactionHeader))]
    public class CreateRecipeMasterDto
    {
        public int? ProductId { get; set; }

        public string ProductCode { get; set; }

        public string MaterialDescription { get; set; }
        public string DocumentVersion { get; set; }
        public string RecipeNo { get; set; }
        public int? ApprovedById { get; set; }
        public int? ApprovedLevelId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ApprovalRemarks { get; set; }
        public string ApprovalStatus { get; set; }
        public bool IsActive { get; set; }
        public List<CreayeRecipeMasterdetail> RecipeTransactionDetails { get; set; }
    }
}
