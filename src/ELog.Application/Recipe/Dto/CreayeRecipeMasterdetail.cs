using Abp.AutoMapper;
using ELog.Core.Entities;
using System.Collections.Generic;

namespace ELog.Application.Recipe.Dto
{
    [AutoMapTo(typeof(RecipeTransactionDetails))]
    public class CreayeRecipeMasterdetail
    {
        public int? RecipeTransHdrId { get; set; }
        public string Operation { get; set; }
        public string Stage { get; set; }
        public string NextOperation { get; set; }
        public string Component { get; set; }
        public bool IsWeightRequired { get; set; }
        public bool IsLebalPrintingRequired { get; set; }
        public bool IsVerificationReq { get; set; }
        public bool InProcessSamplingRequired { get; set; }
        public bool IsSamplingReq { get; set; }
        public bool IsActive { get; set; }
        public string DocumentVersion { get; set; }
        public string RecipeNo { get; set; }
        public string MaterialDescription { get; set; }


        //public RecipeTransactionHeader RecipeTransactionHeader { get; set; }

        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public string ApprovalRemarks { get; set; }


        public List<CreateCubicalRecipeTranDetlMapping> CreateCubicalRecipeTranDetlMapping { get; set; }
        public List<CreateCompRecipeTransDetlMapping> CreateCompRecipeTransDetlMapping { get; set; }

    }
}
